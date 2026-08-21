import struct, uuid, pathlib
from collections import OrderedDict

ROOT = pathlib.Path(__file__).resolve().parents[1]
OUT = ROOT / 'dist' / 'FoATrainer_V18_4.dll'
SRC = (ROOT / 'src' / 'FoATrainerRuntime.cs').read_text(encoding='utf-8')
OUT.parent.mkdir(parents=True, exist_ok=True)

def a(x,n): return (x+n-1)//n*n
def u16(x): return struct.pack('<H',x)
def u32(x): return struct.pack('<I',x)
def u64(x): return struct.pack('<Q',x)
def c_uint(v):
    if v < 0x80: return bytes([v])
    if v < 0x4000: return bytes([0x80 | (v>>8), v & 0xff])
    if v < 0x20000000: return bytes([0xC0 | (v>>24), (v>>16)&0xff, (v>>8)&0xff, v&0xff])
    raise ValueError(v)
def ser_string(s):
    if s is None: return b'\xff'
    b=s.encode('utf-8'); return c_uint(len(b))+b

def sig_class(typeref_rid):
    coded=(typeref_rid<<2)|1
    return b'\x12'+c_uint(coded)

class StringHeap:
    def __init__(self): self.data=bytearray(b'\0'); self.map={'':0}
    def add(self,s):
        if s in self.map:return self.map[s]
        off=len(self.data); self.data.extend(s.encode('utf-8')+b'\0'); self.map[s]=off; return off
class BlobHeap:
    def __init__(self): self.data=bytearray(b'\0'); self.map={b'':0}
    def add(self,b):
        b=bytes(b)
        if b in self.map:return self.map[b]
        off=len(self.data); self.data.extend(c_uint(len(b))); self.data.extend(b); self.map[b]=off; return off
class USHeap:
    def __init__(self): self.data=bytearray(b'\0')
    def add(self,s):
        off=len(self.data); b=s.encode('utf-16le')
        special=1 if any((ord(ch)>0x7f or ord(ch) in list(range(1,9))+list(range(14,32))+[0x27,0x2d,0x7f]) for ch in s) else 0
        payload=b+bytes([special]); self.data.extend(c_uint(len(payload))); self.data.extend(payload); return off

strings=StringHeap(); blobs=BlobHeap(); us=USHeap(); idx={}
for s in ['FoATrainer_V18_4.dll','<Module>','Bootstrap','FoATrainer','.ctor','Awake',
          'Object','System','Assembly','System.Reflection','BaseUnityPlugin','BepInEx','BepInPlugin',
          'File','System.IO','Type','Activator','MethodInfo','StreamWriter','System.IO','Boolean',
          'FieldInfo','PropertyInfo','Module','BindingFlags','Load','GetType','CreateInstance',
          'GetMethod','GetField','GetProperty','GetValue','Invoke','AppendAllText','set_AutoFlush','mscorlib']:
    idx[s]=strings.add(s)

# TypeRef RIDs
TR_OBJECT=1; TR_ASSEMBLY=2; TR_BASEPLUGIN=3; TR_PLUGINATTR=4; TR_FILE=5; TR_TYPE=6; TR_ACTIVATOR=7; TR_METHODINFO=8; TR_STREAMWRITER=9; TR_BOOLEAN=10; TR_FIELDINFO=11; TR_PROPERTYINFO=12; TR_MODULE=13; TR_BINDINGFLAGS=14
# MemberRef RIDs
MR_BASE_CTOR=1; MR_ATTR_CTOR=2; MR_APPENDALLTEXT=3; MR_ASM_LOAD=4; MR_ASM_GETTYPE=5; MR_ACT_CREATE1=6; MR_ACT_CREATE2=7; MR_TYPE_GETMETHOD=8; MR_MI_INVOKE=9; MR_SW_CTOR=10; MR_SW_AUTOFLUSH=11; MR_TYPE_GETFIELD=12; MR_FIELD_GETVALUE=13; MR_OBJECT_GETTYPE=14; MR_TYPE_GETPROPERTY=15; MR_PROPERTY_GETVALUE=16; MR_MODULE_GETTYPE=17

sig_void0=blobs.add(b'\x20\x00\x01')
sig_attr=blobs.add(b'\x20\x03\x01\x0e\x0e\x0e')
sig_append=blobs.add(b'\x00\x02\x01\x0e\x0e')
sig_load=blobs.add(b'\x00\x01'+sig_class(TR_ASSEMBLY)+b'\x0e')
sig_gettype=blobs.add(b'\x20\x01'+sig_class(TR_TYPE)+b'\x0e')
sig_create1=blobs.add(b'\x00\x01\x1c'+sig_class(TR_TYPE))
sig_create2=blobs.add(b'\x00\x02\x1c'+sig_class(TR_TYPE)+b'\x1d\x1c')
sig_getmethod=blobs.add(b'\x20\x01'+sig_class(TR_METHODINFO)+b'\x0e')
sig_invoke=blobs.add(b'\x20\x02\x1c\x1c\x1d\x1c')
sig_sw_ctor=blobs.add(b'\x20\x01\x01\x0e')
sig_autoflush=blobs.add(b'\x20\x01\x01\x02')
sig_getfield=blobs.add(b'\x20\x02'+sig_class(TR_FIELDINFO)+b'\x0e\x11'+c_uint((TR_BINDINGFLAGS<<2)|1))
sig_field_getvalue=blobs.add(b'\x20\x01\x1c\x1c')
sig_object_gettype=blobs.add(b'\x20\x00'+sig_class(TR_TYPE))
sig_getproperty=blobs.add(b'\x20\x01'+sig_class(TR_PROPERTYINFO)+b'\x0e')
sig_property_getvalue=blobs.add(b'\x20\x02\x1c\x1c\x1d\x1c')
sig_module_gettype=blobs.add(b'\x20\x01'+sig_class(TR_TYPE)+b'\x0e')
ca_blob=blobs.add(b'\x01\x00'+ser_string('rijiy.foa.trainer.v18.4')+ser_string('Tainted Grail Trainer by Rijiy V18.4')+ser_string('2.6.4')+b'\x00\x00')
mscorlib_token=blobs.add(bytes.fromhex('b77a5c561934e089'))
# locals: Assembly, Type, object, Type, StreamWriter, object, Type, object, Type, object, MethodInfo
locals_sig=blobs.add(b'\x07\x0b'+sig_class(TR_ASSEMBLY)+sig_class(TR_TYPE)+b'\x1c'+sig_class(TR_TYPE)+sig_class(TR_STREAMWRITER)+b'\x1c'+sig_class(TR_TYPE)+b'\x1c'+sig_class(TR_TYPE)+b'\x1c'+sig_class(TR_METHODINFO))

src_us=us.add(SRC)
asm_names=['System','System.Core','UnityEngine','UnityEngine.CoreModule','UnityEngine.IMGUIModule','UnityEngine.InputLegacyModule','UnityEngine.TextRenderingModule','0Harmony','BepInEx']
asm_us=[us.add(x) for x in asm_names]

TOKEN_MEMBERREF=lambda rid:0x0A000000|rid
TOKEN_US=lambda off:0x70000000|off
TOKEN_TYPEREF=lambda rid:0x01000000|rid
TOKEN_STANDALONESIG=lambda rid:0x11000000|rid

def tok(op,t): return bytes([op])+u32(t)
def log_il(text): return tok(0x72,TOKEN_US(us.add('BepInEx\\FoATrainer_boot.log')))+tok(0x72,TOKEN_US(us.add(text+'\r\n')))+tok(0x28,TOKEN_MEMBERREF(MR_APPENDALLTEXT))
def ldloc(i):
    if i<4:return bytes([0x06+i])
    return b'\x11'+bytes([i])
def stloc(i):
    if i<4:return bytes([0x0A+i])
    return b'\x13'+bytes([i])
def ldc_i4(n):
    if n==0:return b'\x16'
    if n==1:return b'\x17'
    if n==2:return b'\x18'
    if n==3:return b'\x19'
    if n==4:return b'\x1a'
    if n==5:return b'\x1b'
    if n==6:return b'\x1c'
    if n==7:return b'\x1d'
    if n==8:return b'\x1e'
    return b'\x1f'+struct.pack('b',n)
def new_obj_array(count): return ldc_i4(count)+tok(0x8d,TOKEN_TYPEREF(TR_OBJECT))
def arr_set(index, value_il): return b'\x25'+ldc_i4(index)+value_il+b'\xa2'

def activator_create_with_args(type_local, args_ils):
    b=bytearray(); b+=ldloc(type_local); b+=new_obj_array(len(args_ils))
    for i,arg in enumerate(args_ils): b+=arr_set(i,arg)
    b+=tok(0x28,TOKEN_MEMBERREF(MR_ACT_CREATE2)); return bytes(b)

def invoke_method(mi_local, target_il, args_ils):
    b=bytearray(); b+=ldloc(mi_local); b+=target_il; b+=new_obj_array(len(args_ils))
    for i,arg in enumerate(args_ils): b+=arr_set(i,arg)
    b+=tok(0x6f,TOKEN_MEMBERREF(MR_MI_INVOKE)); return bytes(b)

ctor_il=b'\x02'+tok(0x28,TOKEN_MEMBERREF(MR_BASE_CTOR))+b'\x2a'
ctor_body=bytes([(len(ctor_il)<<2)|2])+ctor_il

il=bytearray(); il+=log_il('[FoATrainer.V18.4] Awake entered')
# mcs assembly
il+=tok(0x72,TOKEN_US(us.add('mcs')))+tok(0x28,TOKEN_MEMBERREF(MR_ASM_LOAD))+stloc(0)
il+=log_il('[FoATrainer.V18.4] mcs assembly loaded')
# settings type + instance
il+=ldloc(0)+tok(0x72,TOKEN_US(us.add('Mono.CSharp.CompilerSettings')))+tok(0x6f,TOKEN_MEMBERREF(MR_ASM_GETTYPE))+stloc(1)
il+=ldloc(1)+tok(0x28,TOKEN_MEMBERREF(MR_ACT_CREATE1))+stloc(2)
il+=log_il('[FoATrainer.V18.4] CompilerSettings created')
# writer
il+=tok(0x72,TOKEN_US(us.add('BepInEx\\FoATrainer_compile.log')))+tok(0x73,TOKEN_MEMBERREF(MR_SW_CTOR))+stloc(4)
il+=ldloc(4)+b'\x17'+tok(0x6f,TOKEN_MEMBERREF(MR_SW_AUTOFLUSH))
# report type + instance
il+=ldloc(0)+tok(0x72,TOKEN_US(us.add('Mono.CSharp.StreamReportPrinter')))+tok(0x6f,TOKEN_MEMBERREF(MR_ASM_GETTYPE))+stloc(3)
il+=activator_create_with_args(3,[ldloc(4)])+stloc(5)
il+=log_il('[FoATrainer.V18.4] StreamReportPrinter created')
# context
il+=ldloc(0)+tok(0x72,TOKEN_US(us.add('Mono.CSharp.CompilerContext')))+tok(0x6f,TOKEN_MEMBERREF(MR_ASM_GETTYPE))+stloc(6)
il+=activator_create_with_args(6,[ldloc(2),ldloc(5)])+stloc(7)
il+=log_il('[FoATrainer.V18.4] CompilerContext created')
# evaluator
il+=ldloc(0)+tok(0x72,TOKEN_US(us.add('Mono.CSharp.Evaluator')))+tok(0x6f,TOKEN_MEMBERREF(MR_ASM_GETTYPE))+stloc(8)
il+=activator_create_with_args(8,[ldloc(7)])+stloc(9)
il+=log_il('[FoATrainer.V18.4] Evaluator created')
# get ReferenceAssembly MethodInfo
il+=ldloc(8)+tok(0x72,TOKEN_US(us.add('ReferenceAssembly')))+tok(0x6f,TOKEN_MEMBERREF(MR_TYPE_GETMETHOD))+stloc(10)
# references
for off,name in zip(asm_us,asm_names):
    il+=log_il('[FoATrainer.V18.4] Referencing '+name)
    arg=tok(0x72,TOKEN_US(off))+tok(0x28,TOKEN_MEMBERREF(MR_ASM_LOAD))
    il+=invoke_method(10,ldloc(9),[arg])+b'\x26'
    il+=log_il('[FoATrainer.V18.4] Reference OK '+name)
# get Run MethodInfo
il+=ldloc(8)+tok(0x72,TOKEN_US(us.add('Run')))+tok(0x6f,TOKEN_MEMBERREF(MR_TYPE_GETMETHOD))+stloc(10)
il+=log_il('[FoATrainer.V18.4] Compiling runtime source')
il+=invoke_method(10,ldloc(9),[tok(0x72,TOKEN_US(src_us))])+b'\x26'
il+=log_il('[FoATrainer.V18.4] Locating compiled runtime')
# Evaluator.Run deadlocks on a second submission in the game's Mono.CSharp build.
# Retrieve the emitted ModuleBuilder from Evaluator.module and invoke Start directly.
il+=ldloc(8)+tok(0x72,TOKEN_US(us.add('module')))+ldc_i4(36)+tok(0x6f,TOKEN_MEMBERREF(MR_TYPE_GETFIELD))+stloc(2)
il+=ldloc(2)+tok(0x74,TOKEN_TYPEREF(TR_FIELDINFO))+ldloc(9)+tok(0x6f,TOKEN_MEMBERREF(MR_FIELD_GETVALUE))+stloc(5)
il+=ldloc(5)+tok(0x6f,TOKEN_MEMBERREF(MR_OBJECT_GETTYPE))+stloc(1)
il+=ldloc(1)+tok(0x72,TOKEN_US(us.add('Builder')))+tok(0x6f,TOKEN_MEMBERREF(MR_TYPE_GETPROPERTY))+stloc(2)
il+=ldloc(2)+tok(0x74,TOKEN_TYPEREF(TR_PROPERTYINFO))+ldloc(5)+b'\x14'+tok(0x6f,TOKEN_MEMBERREF(MR_PROPERTY_GETVALUE))+stloc(7)
il+=ldloc(7)+tok(0x74,TOKEN_TYPEREF(TR_MODULE))+tok(0x72,TOKEN_US(us.add('FoATrainerRuntime')))+tok(0x6f,TOKEN_MEMBERREF(MR_MODULE_GETTYPE))+stloc(1)
il+=log_il('[FoATrainer.V18.4] Starting runtime')
il+=ldloc(1)+tok(0x72,TOKEN_US(us.add('Start')))+tok(0x6f,TOKEN_MEMBERREF(MR_TYPE_GETMETHOD))+stloc(10)
il+=invoke_method(10,b'\x14',[])+b'\x26'
il+=log_il('[FoATrainer.V18.4] Awake completed')+b'\x2a'
awake_body=struct.pack('<HHII',0x3013,16,len(il),TOKEN_STANDALONESIG(1))+il  # fat, initlocals

sect=bytearray(); ctor_off=0; sect+=ctor_body
while len(sect)%4:sect+=b'\0'
awake_off=len(sect); sect+=awake_body
while len(sect)%4:sect+=b'\0'
cli_off=len(sect); sect+=b'\0'*72
while len(sect)%4:sect+=b'\0'
meta_off_in_section=len(sect)
SECTION_RVA=0x2000; ctor_rva=SECTION_RVA+ctor_off; awake_rva=SECTION_RVA+awake_off

# metadata tables
# refs: mscorlib=1, BepInEx=2
rows={0:1,1:14,2:2,6:2,10:17,12:1,17:1,32:1,35:2}
valid=sum(1<<t for t in rows); heap_sizes=0
res_scope_aref=lambda rid:(rid<<2)|2
mr_parent_tr=lambda rid:(rid<<3)|1
td_or_ref_tr=lambda rid:(rid<<2)|1

table=bytearray(); table+=u32(0)+bytes([2,0,heap_sizes,1])+u64(valid)+u64(0)
for t in range(64):
    if t in rows:table+=u32(rows[t])
# Module
table+=u16(0)+u16(idx['FoATrainer_V18_4.dll'])+u16(1)+u16(0)+u16(0)
# TypeRefs
trs=[
 (1,'Object','System'),(1,'Assembly','System.Reflection'),(2,'BaseUnityPlugin','BepInEx'),(2,'BepInPlugin','BepInEx'),
 (1,'File','System.IO'),(1,'Type','System'),(1,'Activator','System'),(1,'MethodInfo','System.Reflection'),
 (1,'StreamWriter','System.IO'),(1,'Boolean','System'),(1,'FieldInfo','System.Reflection'),
 (1,'PropertyInfo','System.Reflection'),(1,'Module','System.Reflection'),(1,'BindingFlags','System.Reflection')]
for ar,name,ns in trs: table+=u16(res_scope_aref(ar))+u16(idx[name])+u16(idx[ns])
# TypeDefs
table+=u32(0)+u16(idx['<Module>'])+u16(0)+u16(0)+u16(1)+u16(1)
table+=u32(0x00100001)+u16(idx['Bootstrap'])+u16(idx['FoATrainer'])+u16(td_or_ref_tr(TR_BASEPLUGIN))+u16(1)+u16(1)
# methods
table+=u32(ctor_rva)+u16(0)+u16(0x1886)+u16(idx['.ctor'])+u16(sig_void0)+u16(1)
table+=u32(awake_rva)+u16(0)+u16(0x0081)+u16(idx['Awake'])+u16(sig_void0)+u16(1)
# MemberRefs
mrs=[
 (TR_BASEPLUGIN,'.ctor',sig_void0),(TR_PLUGINATTR,'.ctor',sig_attr),(TR_FILE,'AppendAllText',sig_append),
 (TR_ASSEMBLY,'Load',sig_load),(TR_ASSEMBLY,'GetType',sig_gettype),(TR_ACTIVATOR,'CreateInstance',sig_create1),(TR_ACTIVATOR,'CreateInstance',sig_create2),
 (TR_TYPE,'GetMethod',sig_getmethod),(TR_METHODINFO,'Invoke',sig_invoke),(TR_STREAMWRITER,'.ctor',sig_sw_ctor),(TR_STREAMWRITER,'set_AutoFlush',sig_autoflush),
 (TR_TYPE,'GetField',sig_getfield),(TR_FIELDINFO,'GetValue',sig_field_getvalue),(TR_OBJECT,'GetType',sig_object_gettype),
 (TR_TYPE,'GetProperty',sig_getproperty),(TR_PROPERTYINFO,'GetValue',sig_property_getvalue),(TR_MODULE,'GetType',sig_module_gettype)]
for tr,name,sg in mrs: table+=u16(mr_parent_tr(tr))+u16(idx[name])+u16(sg)
# custom attr
hasca=(2<<5)|3; catype=(MR_ATTR_CTOR<<3)|3; table+=u16(hasca)+u16(catype)+u16(ca_blob)
# StandAloneSig
table+=u16(locals_sig)
# Assembly
table+=u32(0x00008004)+u16(1)+u16(0)+u16(0)+u16(0)+u32(0)+u16(0)+u16(idx['FoATrainer'])+u16(0)
# AssemblyRefs
arefs=[(4,0,0,0,0,mscorlib_token,'mscorlib'),(5,4,23,3,0,0,'BepInEx')]
for ma,mi,bu,re,fl,pkt,name in arefs:
    table+=u16(ma)+u16(mi)+u16(bu)+u16(re)+u32(fl)+u16(pkt)+u16(idx[name])+u16(0)+u16(0)

streams=OrderedDict([('#~',bytes(table)),('#Strings',bytes(strings.data)),('#US',bytes(us.data)),('#GUID',uuid.uuid4().bytes_le),('#Blob',bytes(blobs.data))])
version=b'v2.0.50727\0\0'; root_fixed=b'BSJB'+u16(1)+u16(1)+u32(0)+u32(len(version))+version+u16(0)+u16(len(streams))
header_size=len(root_fixed)
for name,data in streams.items(): header_size+=8+a(len(name)+1,4)
cur=a(header_size,4); stream_offsets={}
for name,data in streams.items(): cur=a(cur,4); stream_offsets[name]=cur; cur+=len(data)
meta=bytearray(root_fixed)
for name,data in streams.items():
    meta+=u32(stream_offsets[name])+u32(len(data)); nb=name.encode()+b'\0'; meta+=nb+b'\0'*(a(len(nb),4)-len(nb))
meta+=b'\0'*(min(stream_offsets.values())-len(meta))
for name,data in streams.items():
    target=stream_offsets[name]
    if len(meta)<target:meta+=b'\0'*(target-len(meta))
    assert len(meta)==target,(name,len(meta),target)
    meta+=data
meta=bytes(meta)
assert len(sect)==meta_off_in_section; sect+=meta
metadata_rva=SECTION_RVA+meta_off_in_section; cli_rva=SECTION_RVA+cli_off
cli=struct.pack('<IHHIIIII',72,2,5,metadata_rva,len(meta),1,0,0); cli+=b'\0'*(72-len(cli))
sect[cli_off:cli_off+72]=cli

# PE32 one section (AnyCPU-style, matching normal Unity/BepInEx plugins)
FILE_ALIGN=0x200; SECT_ALIGN=0x2000; HEADERS=0x200
raw_size=a(len(sect),FILE_ALIGN); virt_size=len(sect); size_image=a(SECTION_RVA+virt_size,SECT_ALIGN)
sect_padded=sect+b'\0'*(raw_size-len(sect))
dos=bytearray(0x80); dos[0:2]=b'MZ'; struct.pack_into('<I',dos,0x3c,0x80)
stub=b'This program cannot be run in DOS mode.\r\r\n$'; dos[0x40:0x40+len(stub)]=stub
pe=bytearray(b'PE\0\0'); pe+=struct.pack('<HHIIIHH',0x14c,1,0,0,0,0xE0,0x2102)
opt=bytearray(0xE0); struct.pack_into('<H',opt,0,0x10b); opt[2]=8; opt[3]=0
struct.pack_into('<III',opt,4,raw_size,0,0); struct.pack_into('<III',opt,16,0,SECTION_RVA,0)
struct.pack_into('<I',opt,28,0x400000); struct.pack_into('<II',opt,32,SECT_ALIGN,FILE_ALIGN)
struct.pack_into('<HHHHHH',opt,40,4,0,0,0,4,0); struct.pack_into('<I',opt,52,0)
struct.pack_into('<II',opt,56,size_image,HEADERS); struct.pack_into('<I',opt,64,0)
struct.pack_into('<HH',opt,68,3,0x8540); struct.pack_into('<IIIIII',opt,72,0x100000,0x1000,0x100000,0x1000,0,16)
struct.pack_into('<II',opt,96+14*8,cli_rva,72); pe+=opt
pe+=b'.text\0\0\0'+struct.pack('<IIIIIIHHI',virt_size,SECTION_RVA,raw_size,HEADERS,0,0,0,0,0x60000020)
headers=bytes(dos)+bytes(pe); headers+=b'\0'*(HEADERS-len(headers))
out=headers+bytes(sect_padded); pathlib.Path(OUT).write_bytes(out)
print(OUT,len(out),'IL',len(il),'meta',len(meta),'src chars',len(SRC))
