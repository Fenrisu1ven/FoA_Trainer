public class FoATrainerRuntime : UnityEngine.MonoBehaviour
{
    public static FoATrainerRuntime Instance;
    static HarmonyLib.Harmony _harmony;
    static BepInEx.Logging.ManualLogSource _log;
    static System.Reflection.BindingFlags AllFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;

    public static bool GodMode;
    public static bool InfiniteHealth;
    public static bool InfiniteMana;
    public static bool InfiniteStamina;
    public static bool InfiniteKingsPower;
    public static bool InfiniteOxygen;
    public static bool ItemsWontDecrease;
    public static bool StealthMode;
    public static bool EasyLockPicking;
    public static bool OneHitKills;
    public static bool DamageMultiplierEnabled;
    public static bool DefenseMultiplierEnabled;
    public static bool ManaRateEnabled;
    public static bool StaminaRateEnabled;
    public static bool ZeroItemWeight;
    public static bool ZeroEquipmentWeight;
    public static bool IgnoreCraftingRequirement;
    public static bool InfiniteExp;
    public static bool ExpMultiplierEnabled;
    public static bool InfiniteProfExp;
    public static bool ProfExpMultiplierEnabled;
    public static bool GameSpeedEnabled;
    public static bool MovementSpeedEnabled;
    public static bool JumpHeightEnabled;
    public static bool NoFallDamage;
    public static bool FreezeDaytime;
    public static bool TimePassSpeedEnabled;
    public static bool FlightEnabled;

    // V18.4: stability branch based directly on working V18.3. Adds lightweight icon badges, compact optional HP bars, loot state, empty-loot filtering and corrected corpse handling.
    public static bool EspEnabled;
    public static bool EspItems = true;
    public static bool EspContainers = true;
    public static bool EspEnemies = true;
    public static bool EspNpcs = false;
    public static bool EspFriendlies = true;
    public static bool EspMerchants = true;
    public static bool EspShowItemWeapons = true;
    public static bool EspShowItemArmor = true;
    public static bool EspShowItemConsumables = true;
    public static bool EspShowItemMaterials = true;
    public static bool EspShowItemImportant = true;
    public static bool EspShowItemOther = false;
    public static bool EspShowNames = true;
    public static bool EspShowDistance = true;
    public static bool EspShowHealth = true;
    public static bool EspShowHealthBars = true;
    public static bool EspShowDead = false;
    public static bool EspShowLootState = true;
    public static bool EspHideEmptyLoot = false;
    public static bool EspShowBackground = true;
    public static bool EspShowIcons = true;
    public static bool EspIconsOnly = false;
    public static float EspItemDistance = 65.0f;
    public static float EspContainerDistance = 90.0f;
    public static float EspEnemyDistance = 180.0f;
    public static float EspNpcDistance = 120.0f;
    public static float EspScanInterval = 0.75f;
    public static int EspFontSize = 13;
    public static int EspIconSize = 20;
    public static int EspHealthBarWidth = 52;
    public static int EspHealthBarHeight = 3;
    public static int EspMaxObjects = 120;

    // V17.1: interface language, 0 = English, 1 = Russian. Public so profiles persist it.
    public static int Language = 0;

    public static float DamageMultiplier = 2.0f;
    public static float DefenseMultiplier = 2.0f;
    public static float ManaRate = 0.5f;
    public static float StaminaRate = 0.5f;
    public static float ExpMultiplier = 2.0f;
    public static float ProfExpMultiplier = 2.0f;
    public static float GameSpeed = 2.5f;
    public static float MovementSpeed = 2.0f;
    public static float JumpHeight = 2.0f;
    public static float TimePassSpeed = 0.5f;
    public static float FlightSpeed = 10.0f;
    public static float FlightBoost = 3.0f;

    public static int CobwebValue = 9999;
    public static int MoneyValue = 999999;
    public static int PotionAmount = 99;
    public static int ConsumablesAmount = 99;
    public static int MaterialsAmount = 999;
    public static int SelectedItemAmount = 99;
    public static int SelectedItemLevel = 10;
    public static int PlayerLevel = 1;
    public static int AttributePoints = 0;
    public static int SkillPoints = 0;

    public static int StrengthValue = 1;
    public static int EnduranceValue = 1;
    public static int DexterityValue = 1;
    public static int SpiritualityValue = 1;
    public static int PracticalityValue = 1;
    public static int PerceptionValue = 1;

    public static int OneHandedValue = 1;
    public static int TwoHandedValue = 1;
    public static int UnarmedValue = 1;
    public static int BlockingValue = 1;
    public static int AthleticsValue = 1;
    public static int LightArmorValue = 1;
    public static int MediumArmorValue = 1;
    public static int HeavyArmorValue = 1;
    public static int ArcheryValue = 1;
    public static int EvasionValue = 1;
    public static int AgilityValue = 1;
    public static int SneakValue = 1;
    public static int TheftValue = 1;
    public static int MagicValue = 1;
    public static int AlchemyValue = 1;
    public static int CookingValue = 1;
    public static int HandcraftingValue = 1;

    static object _selectedItem;
    static object _lastHero;
    static bool _menuVisible = true;
    static UnityEngine.Rect _windowRect = new UnityEngine.Rect(45f, 45f, 1280f, 960f);
    static UnityEngine.Vector2 _scroll = UnityEngine.Vector2.zero;
    static int _tab;
    static int _lastMenuToggleFrame = -100;
    static bool _windowPositioned;
    static UnityEngine.CursorLockMode _savedCursorLock;
    static bool _savedCursorVisible;
    static int _patchOk;
    static System.Collections.Generic.List<string> _patchErrors = new System.Collections.Generic.List<string>();
    static string _lastAction = "Готов";

    // V8: flight
    static object _flightController;
    static bool _flightControllerSaved;
    static bool _flightControllerWasEnabled = true;

    // V8: profiles
    static string _profileName = "My profile";
    static System.Collections.Generic.List<string> _profiles = new System.Collections.Generic.List<string>();
    static bool _profilesScanned;
    static bool _autoProfileLoadAttempted;
    static float _startupProfileLoadAfter;
    static int _startupProfileLoadAttempts;
    static string _startupProfileName;

    // V8: item spawner
    static System.Collections.Generic.List<object> _itemTemplates = new System.Collections.Generic.List<object>();
    static bool _itemTemplatesLoaded;
    static string _itemTemplatesStatus = "База предметов еще не загружена";
    static string _itemSearch = "";
    static object _spawnTemplate;
    static int _spawnQuantity = 1;
    static int _spawnLevel = 1;
    static bool _showHiddenItems;
    static string _spawnPreview = "Выберите предмет из списка";
    static UnityEngine.Vector2 _spawnListScroll = UnityEngine.Vector2.zero;
    static int _itemGroup = 0;
    static int _itemSubtype = 0;
    static int _itemDetail = 0;
    static string[] _itemGroupNames = new string[] { "Все", "Оружие", "Броня", "Щиты", "Расходники", "Материалы", "Украшения", "Самоцветы", "Книги", "Ключи", "Инструменты", "Важные", "Прочее" };
    static string[] _weaponSubtypeNames = new string[] { "Все", "Ближнее", "Одноручное", "Двуручное", "Кинжалы", "Мечи", "Топоры", "Дробящее", "Древковое", "Дальнее", "Луки", "Стрелы", "Метательное", "Магия", "Жезлы", "Кулаки", "Спектральное", "Chaingun", "Soul Cube" };
    static string[] _bowDetailNames = new string[] { "Все луки", "Короткие", "Средние", "Тяжелые" };
    static string[] _armorSubtypeNames = new string[] { "Все", "Легкая", "Средняя", "Тяжелая" };
    static string[] _consumableSubtypeNames = new string[] { "Все", "Зелья", "Еда", "Блюда", "Рыба", "Алкоголь", "Лечение", "Мана", "Выносливость", "Баффы" };
    static string[] _materialSubtypeNames = new string[] { "Все", "Алхимия", "Готовка", "Крафт", "Компоненты" };
    static string[] _singleSubtypeNames = new string[] { "Все" };

    // V18.2: optimized ESP cache. ModelsSet is converted through GetManagedEnumerator; reflection accessors are cached.
    // OnGUI uses cached coordinates/anchors and draws only during Repaint.
    enum EspEntityType
    {
        Item = 1,
        Container = 2,
        Hostile = 3,
        Neutral = 4,
        Friendly = 5,
        Merchant = 6
    }

    class EspEntry
    {
        public object Source;
        public object Template;
        public UnityEngine.Transform Anchor;
        public UnityEngine.Vector3 Position;
        public UnityEngine.Vector3 AnchorOffset;
        public EspEntityType Kind;
        public string Name;
        public float Distance;
        public string HealthText;
        public float HealthRatio;
        public string IconText;
        public string StateText;
        public bool IsDead;
        public bool IsEmpty;
    }
    static System.Collections.Generic.List<EspEntry> _espEntries = new System.Collections.Generic.List<EspEntry>();
    static System.Collections.Generic.Dictionary<string, System.Reflection.MethodInfo> _espWorldAllMethods = new System.Collections.Generic.Dictionary<string, System.Reflection.MethodInfo>();
    static System.Collections.Generic.Dictionary<string, System.Reflection.MethodInfo> _espWorldManagedMethods = new System.Collections.Generic.Dictionary<string, System.Reflection.MethodInfo>();
    static System.Collections.Generic.Dictionary<string, System.Reflection.MemberInfo> _espMemberCache = new System.Collections.Generic.Dictionary<string, System.Reflection.MemberInfo>();
    static System.Collections.Generic.Dictionary<string, bool> _espMissingMemberCache = new System.Collections.Generic.Dictionary<string, bool>();
    static System.Collections.Generic.Dictionary<string, int> _espNpcLocationState = new System.Collections.Generic.Dictionary<string, int>();
    static System.Collections.Generic.Dictionary<object, bool> _espMerchantLocations = new System.Collections.Generic.Dictionary<object, bool>();
    static System.Collections.Generic.Dictionary<string, bool> _espMerchantLocationIds = new System.Collections.Generic.Dictionary<string, bool>();
    static System.Reflection.FieldInfo _espPickItemDataField;
    static float _espNextScan;
    static int _espVisibleLastFrame;
    static int _espItemsCached;
    static int _espContainersCached;
    static int _espEnemiesCached;
    static int _espNpcsCached;
    static int _espFriendliesCached;
    static int _espMerchantsCached;
    static int _espCorpsesCached;
    static int _espItemsRaw;
    static int _espContainersRaw;
    static int _espNpcsRaw;
    static UnityEngine.Camera _espCamera;
    static float _espNextCameraResolve;
    static string _espCameraName = "-";
    static string _espStatus = "ESP готов";

    // V19: native weather control. Weather settings intentionally live in BepInEx Config,
    // not in trainer profiles, so older profiles remain compatible and cannot silently
    // overwrite the user's weather preference.
    static BepInEx.Configuration.ConfigFile _weatherConfig;
    static BepInEx.Configuration.ConfigEntry<bool> _weatherOverrideConfig;
    static BepInEx.Configuration.ConfigEntry<int> _weatherPresetConfig;
    static bool _weatherOverrideEnabled;
    static int _selectedWeatherPreset;
    static object _weatherController;
    static System.Array _weatherPresets;
    static System.Collections.Generic.List<string> _weatherPresetNames = new System.Collections.Generic.List<string>();
    static System.Reflection.FieldInfo _weatherPresetsField;
    static System.Reflection.FieldInfo _weatherCurrentIndexField;
    static System.Reflection.FieldInfo _weatherPresetNameField;
    static System.Reflection.MethodInfo _weatherSetPresetMethod;
    static float _weatherNextRefresh;
    static int _weatherCurrentIndex = -1;
    static float _weatherPrecipitationIntensity;
    static float _weatherRainIntensity;
    static float _weatherSnowIntensity;
    static bool _weatherHeavyRain;
    static bool _weatherDropdownOpen;
    static string _weatherStatus = "Погодная система еще не загружена";

    static bool _stealthSaved;
    static float _visOrig, _crouchVisOrig, _noiseOrig, _crouchNoiseOrig;
    static bool _lockSaved;
    static float _lockDamageOrig, _lockToleranceOrig;
    static bool _manaRateSaved;
    static float _manaRateOrig;
    static bool _staminaRateSaved;
    static float _staminaRateOrig;
    static bool _movementSaved;
    static float _movementOrig;
    static bool _jumpSaved;
    static float _jumpOrig;
    static bool _gameSpeedSaved;
    static float _gameSpeedOrig = 1f;

    static UnityEngine.GUIStyle _windowStyle;
    static UnityEngine.GUIStyle _titleStyle;
    static UnityEngine.GUIStyle _subtitleStyle;
    static UnityEngine.GUIStyle _sectionStyle;
    static UnityEngine.GUIStyle _statusStyle;
    static UnityEngine.GUIStyle _tabStyle;
    static UnityEngine.GUIStyle _tabActiveStyle;
    static UnityEngine.GUIStyle _rowStyle;
    static UnityEngine.GUIStyle _toggleStyle;
    static UnityEngine.GUIStyle _toggleMarkerOnStyle;
    static UnityEngine.GUIStyle _toggleMarkerOffStyle;
    static UnityEngine.GUIStyle _toggleLabelStyle;
    static UnityEngine.GUIStyle _actionMarkerStyle;
    static UnityEngine.GUIStyle _actionLabelStyle;
    static UnityEngine.GUIStyle _creatorLinkStyle;
    static UnityEngine.GUIStyle _textFieldStyle;
    static UnityEngine.GUIStyle _buttonStyle;
    static UnityEngine.GUIStyle _dangerButtonStyle;
    static UnityEngine.GUIStyle _orangeButtonStyle;
    static UnityEngine.GUIStyle _hotkeyStyle;
    static UnityEngine.GUIStyle _footerStyle;
    static UnityEngine.GUIStyle _goodStatusStyle;
    static UnityEngine.GUIStyle _badStatusStyle;
    static UnityEngine.GUIStyle _headerStyle;
    static UnityEngine.GUIStyle _contentStyle;
    static UnityEngine.GUIStyle _cardStyle;
    static UnityEngine.GUIStyle _resizeGripStyle;
    static UnityEngine.GUIStyle _espTextStyle;
    static UnityEngine.Texture2D _texEspBg;
    static UnityEngine.Texture2D _texWindow;
    static UnityEngine.Texture2D _texPanel;
    static UnityEngine.Texture2D _texPanelAlt;
    static UnityEngine.Texture2D _texAccent;
    static UnityEngine.Texture2D _texAccentDark;
    static UnityEngine.Texture2D _texInput;
    static UnityEngine.Texture2D _texDanger;
    static UnityEngine.Texture2D _texOrange;
    static UnityEngine.Texture2D _texHeader;
    static UnityEngine.Texture2D _texContent;
    static UnityEngine.Texture2D _texCard;
    static bool _resizingWindow;
    static UnityEngine.Vector2 _resizeStartMouseScreen;
    static UnityEngine.Vector2 _resizeStartWindowSize;
    static bool _windowFullscreen;
    static UnityEngine.Rect _windowRectBeforeFullscreen;
    const float MinWindowWidth = 760f;
    const float MinWindowHeight = 600f;

    // IMGUI redraws every frame. Keep the user's raw text while a numeric field is focused so
    // intermediate values such as "", "-", "1." and "1," are not replaced prematurely.
    class NumericInputState
    {
        public string Text;
        public bool WasFocused;
        public bool Initialized;
        public float LastFloatValue;
        public int LastIntValue;
    }
    static System.Collections.Generic.Dictionary<string, NumericInputState> _numericInputStates = new System.Collections.Generic.Dictionary<string, NumericInputState>();

    public static void Start()
    {
        try
        {
            if (Instance != null) return;
            if (_log == null) _log = BepInEx.Logging.Logger.CreateLogSource("FoATrainer.Runtime");
            UnityEngine.GameObject go = new UnityEngine.GameObject("FoATrainer.Runtime");
            UnityEngine.Object.DontDestroyOnLoad(go);
            Instance = go.AddComponent<FoATrainerRuntime>();
            Instance.InitializeTrainer();
        }
        catch (System.Exception ex)
        {
            if (_log != null) _log.LogError("[FoATrainer] Start error: " + ex);
            throw;
        }
    }

    void InitializeTrainer()
    {
        try
        {
            _savedCursorLock = UnityEngine.Cursor.lockState;
            _savedCursorVisible = UnityEngine.Cursor.visible;
            if (_menuVisible)
            {
                UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
            _harmony = new HarmonyLib.Harmony("openai.foa.trainer.runtime");
            InstallPatches();
            RefreshProfiles();
            PrepareStartupProfile();
            PrimeStartupProfile();
            InitializeWeatherConfig();
            _log.LogInfo("[FoATrainer] Runtime started. Patches: " + _patchOk + ", missing: " + _patchErrors.Count);
        }
        catch (System.Exception ex)
        {
            _log.LogError("[FoATrainer] Init error: " + ex);
        }
    }

    void OnDestroy()
    {
        try
        {
            SetFlight(false);
            RestoreAllTemporaryStats();
            if (_harmony != null) _harmony.UnpatchSelf();
        }
        catch { }
    }

    static System.Type FindType(string fullName)
    {
        System.Reflection.Assembly[] all = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < all.Length; i++)
        {
            try
            {
                System.Type t = all[i].GetType(fullName, false);
                if (t != null) return t;
            }
            catch { }
        }
        return null;
    }

    static System.Reflection.FieldInfo FindField(System.Type t, string name)
    {
        while (t != null)
        {
            System.Reflection.FieldInfo f = t.GetField(name, AllFlags | System.Reflection.BindingFlags.DeclaredOnly);
            if (f != null) return f;
            t = t.BaseType;
        }
        return null;
    }

    static System.Reflection.PropertyInfo FindProperty(System.Type t, string name, bool requireSetter)
    {
        while (t != null)
        {
            System.Reflection.PropertyInfo p = t.GetProperty(name, AllFlags | System.Reflection.BindingFlags.DeclaredOnly);
            if (p != null && (!requireSetter || p.CanWrite)) return p;
            t = t.BaseType;
        }
        return null;
    }

    static System.Reflection.MethodInfo FindMethod(System.Type t, string name, int parameterCount)
    {
        while (t != null)
        {
            System.Reflection.MethodInfo[] ms = t.GetMethods(AllFlags | System.Reflection.BindingFlags.DeclaredOnly);
            for (int i = 0; i < ms.Length; i++)
            {
                if (ms[i].Name == name && ms[i].GetParameters().Length == parameterCount) return ms[i];
            }
            t = t.BaseType;
        }
        return null;
    }

    static System.Reflection.MethodInfo FindMethodByParameterType(System.Type t, string name, int parameterCount, int parameterIndex, string parameterTypeName)
    {
        while (t != null)
        {
            System.Reflection.MethodInfo[] ms = t.GetMethods(AllFlags | System.Reflection.BindingFlags.DeclaredOnly);
            for (int i = 0; i < ms.Length; i++)
            {
                if (ms[i].Name != name) continue;
                System.Reflection.ParameterInfo[] ps = ms[i].GetParameters();
                if (ps.Length != parameterCount) continue;
                if (parameterIndex < 0 || parameterIndex >= ps.Length) continue;
                System.Type pt = ps[parameterIndex].ParameterType;
                if (pt != null && pt.FullName == parameterTypeName) return ms[i];
            }
            t = t.BaseType;
        }
        return null;
    }

    static object GetProp(object obj, string name)
    {
        if (obj == null) return null;
        try
        {
            System.Reflection.PropertyInfo p = FindProperty(obj.GetType(), name, false);
            if (p != null) return p.GetValue(obj, null);
            System.Reflection.FieldInfo f = FindField(obj.GetType(), name);
            if (f != null) return f.GetValue(obj);
        }
        catch { }
        return null;
    }

    static object GetStaticProp(System.Type t, string name)
    {
        if (t == null) return null;
        try
        {
            System.Reflection.PropertyInfo p = FindProperty(t, name, false);
            if (p != null) return p.GetValue(null, null);
            System.Reflection.FieldInfo f = FindField(t, name);
            if (f != null) return f.GetValue(null);
        }
        catch { }
        return null;
    }

    static object Hero()
    {
        return GetStaticProp(FindType("Awaken.TG.Main.Heroes.Hero"), "Current");
    }

    static float ToFloat(object value, float fallback)
    {
        if (value == null) return fallback;
        try { return System.Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture); }
        catch { return fallback; }
    }

    static int ToInt(object value, int fallback)
    {
        if (value == null) return fallback;
        try { return System.Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture); }
        catch { return fallback; }
    }

    static bool ToBool(object value)
    {
        if (value == null) return false;
        try { return System.Convert.ToBoolean(value, System.Globalization.CultureInfo.InvariantCulture); }
        catch { return false; }
    }

    static float StatBase(object stat)
    {
        if (stat == null) return 0f;
        System.Reflection.FieldInfo f = FindField(stat.GetType(), "<BaseValue>k__BackingField");
        if (f != null)
        {
            try { return ToFloat(f.GetValue(stat), 0f); } catch { }
        }
        return ToFloat(GetProp(stat, "BaseValue"), 0f);
    }

    static float StatModified(object stat)
    {
        if (stat == null) return 0f;
        object v = GetProp(stat, "ModifiedValue");
        if (v != null) return ToFloat(v, StatBase(stat));
        return StatBase(stat);
    }

    static float StatUpper(object stat)
    {
        if (stat == null) return 0f;
        object v = GetProp(stat, "UpperLimit");
        if (v != null) return ToFloat(v, StatModified(stat));
        return StatModified(stat);
    }

    static void ClearStatCaches(object stat)
    {
        if (stat == null) return;
        try
        {
            System.Reflection.FieldInfo f = FindField(stat.GetType(), "<CachedModifiedValue>k__BackingField");
            if (f != null) f.SetValue(stat, null);
        }
        catch { }
        try
        {
            System.Reflection.FieldInfo f = FindField(stat.GetType(), "_cached");
            if (f != null) f.SetValue(stat, null);
        }
        catch { }
    }

    static void SetStatRaw(object stat, float value)
    {
        if (stat == null || float.IsNaN(value) || float.IsInfinity(value)) return;
        try
        {
            System.Reflection.FieldInfo f = FindField(stat.GetType(), "<BaseValue>k__BackingField");
            if (f != null)
            {
                f.SetValue(stat, value);
                ClearStatCaches(stat);
                System.Reflection.MethodInfo trigger = FindMethod(stat.GetType(), "TriggerStatChanged", 0);
                if (trigger != null) trigger.Invoke(stat, null);
                return;
            }
            System.Reflection.PropertyInfo p = FindProperty(stat.GetType(), "BaseValue", true);
            if (p != null)
            {
                p.SetValue(stat, value, null);
                ClearStatCaches(stat);
            }
        }
        catch (System.Exception ex)
        {
            _log.LogWarning("[FoATrainer] SetStatRaw: " + ex.Message);
        }
    }

    static void FillStat(object stat)
    {
        if (stat == null) return;
        float upper = StatUpper(stat);
        if (upper > -100000000f && upper < 100000000f) SetStatRaw(stat, upper);
    }

    static object HeroStats(object hero) { return GetProp(hero, "HeroStats"); }
    static object CharacterStats(object hero) { return GetProp(hero, "CharacterStats"); }
    static object RPGStats(object hero) { return GetProp(hero, "HeroRPGStats"); }
    static object ProfStats(object hero) { return GetProp(hero, "ProficiencyStats"); }

    void Update()
    {
        try
        {
            HandleHotkeys();
            HandleContinuousResize();
            MaintainWeatherControl();
            if (_menuVisible)
            {
                UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
            object hero = Hero();
            if (!object.ReferenceEquals(hero, _lastHero))
            {
                _lastHero = hero;
                ResetSavedTemporaries();
                if (hero != null)
                {
                    SyncEditorsFromHero(hero);
                }
            }
            if (hero == null)
            {
                if (_espEntries.Count > 0) _espEntries.Clear();
                return;
            }
            TryAutoLoadStartupProfile(hero);
            if (EspEnabled) UpdateEspCache(hero);
            else if (_espEntries.Count > 0) _espEntries.Clear();
            MaintainCheats(hero);
        }
        catch (System.Exception ex)
        {
            _log.LogWarning("[FoATrainer] Update error: " + ex.Message);
        }
    }

    static System.Collections.IEnumerable WorldAll(string typeName)
    {
        try
        {
            System.Reflection.MethodInfo gm = null;
            if (!_espWorldAllMethods.TryGetValue(typeName, out gm) || gm == null)
            {
                System.Type modelType = FindType(typeName);
                System.Type worldType = FindType("Awaken.TG.MVC.World");
                if (modelType == null || worldType == null)
                {
                    _espStatus = "ESP type not found: " + typeName;
                    return null;
                }
                System.Reflection.MethodInfo[] methods = worldType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                for (int i = 0; i < methods.Length; i++)
                {
                    System.Reflection.MethodInfo m = methods[i];
                    if (m.Name != "All" || !m.IsGenericMethodDefinition) continue;
                    if (m.GetParameters().Length != 0) continue;
                    gm = m.MakeGenericMethod(new System.Type[] { modelType });
                    _espWorldAllMethods[typeName] = gm;
                    break;
                }
            }
            if (gm == null)
            {
                _espStatus = "ESP World.All not found";
                return null;
            }

            // World.All<T>() returns ModelsSet<T>. ModelsSet<T> intentionally does not implement
            // IEnumerable directly; its GetManagedEnumerator() does. The old implementation used
            // `as IEnumerable`, so every ESP scan silently received null.
            object set = gm.Invoke(null, null);
            if (set == null) return null;
            System.Collections.IEnumerable direct = set as System.Collections.IEnumerable;
            if (direct != null) return direct;

            System.Reflection.MethodInfo managed = null;
            if (!_espWorldManagedMethods.TryGetValue(typeName, out managed) || managed == null)
            {
                managed = set.GetType().GetMethod("GetManagedEnumerator", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (managed != null) _espWorldManagedMethods[typeName] = managed;
            }
            if (managed != null)
            {
                object enumerable = managed.Invoke(set, null);
                System.Collections.IEnumerable result = enumerable as System.Collections.IEnumerable;
                if (result != null) return result;
            }
            _espStatus = "ESP ModelsSet enumerator unavailable: " + typeName;
        }
        catch (System.Exception ex)
        {
            _espStatus = "ESP scan error: " + ex.Message;
        }
        return null;
    }

    // ============================ V19: native weather ============================
    static void InitializeWeatherConfig()
    {
        if (_weatherConfig != null) return;
        try
        {
            UnityEngine.Object[] plugins = UnityEngine.Object.FindObjectsOfType(typeof(BepInEx.BaseUnityPlugin));
            for (int i = 0; i < plugins.Length; i++)
            {
                BepInEx.BaseUnityPlugin plugin = plugins[i] as BepInEx.BaseUnityPlugin;
                if (plugin == null || plugin.Info == null || plugin.Info.Metadata == null) continue;
                if (plugin.Info.Metadata.GUID == "rijiy.foa.trainer.v19")
                {
                    _weatherConfig = plugin.Config;
                    break;
                }
            }
            if (_weatherConfig == null)
            {
                string path = System.IO.Path.Combine(BepInEx.Paths.ConfigPath, "rijiy.foa.trainer.v19.weather.cfg");
                _weatherConfig = new BepInEx.Configuration.ConfigFile(path, true);
            }

            _weatherOverrideConfig = _weatherConfig.Bind<bool>(
                "Weather",
                "ForcePreset",
                false,
                "Hold the selected native weather preset. False leaves weather under normal game control.");
            _weatherPresetConfig = _weatherConfig.Bind<int>(
                "Weather",
                "PresetIndex",
                0,
                "Zero-based index from the native WeatherController preset list.");
            _weatherOverrideEnabled = _weatherOverrideConfig.Value;
            _selectedWeatherPreset = System.Math.Max(0, _weatherPresetConfig.Value);
        }
        catch (System.Exception ex)
        {
            _weatherStatus = "Ошибка BepInEx Config: " + ex.Message;
            if (_log != null) _log.LogWarning("[FoATrainer] Weather config error: " + ex.Message);
        }
    }

    static void SaveWeatherConfig()
    {
        try
        {
            if (_weatherOverrideConfig != null) _weatherOverrideConfig.Value = _weatherOverrideEnabled;
            if (_weatherPresetConfig != null) _weatherPresetConfig.Value = _selectedWeatherPreset;
            if (_weatherConfig != null) _weatherConfig.Save();
        }
        catch (System.Exception ex)
        {
            if (_log != null) _log.LogWarning("[FoATrainer] Weather config save error: " + ex.Message);
        }
    }

    static object FindWeatherController()
    {
        System.Collections.IEnumerable all = WorldAll("Awaken.TG.Graphics.WeatherController");
        if (all == null) return null;
        try
        {
            foreach (object model in all)
            {
                if (model != null && !IsDiscarded(model)) return model;
            }
        }
        catch (System.Exception ex)
        {
            _weatherStatus = "Ошибка поиска WeatherController: " + ex.Message;
        }
        return null;
    }

    static void ResetWeatherAccessors(object controller)
    {
        _weatherController = controller;
        _weatherPresets = null;
        _weatherPresetNames.Clear();
        _weatherCurrentIndex = -1;
        _weatherPresetsField = null;
        _weatherCurrentIndexField = null;
        _weatherPresetNameField = null;
        _weatherSetPresetMethod = null;
        if (controller == null) return;

        System.Type type = controller.GetType();
        _weatherPresetsField = FindField(type, "_presets");
        _weatherCurrentIndexField = FindField(type, "_currentIndex");
        _weatherSetPresetMethod = FindMethod(type, "SetPreset", 1);
    }

    static void RefreshWeatherPresets()
    {
        if (_weatherController == null || _weatherPresetsField == null) return;
        System.Array presets = null;
        try { presets = _weatherPresetsField.GetValue(_weatherController) as System.Array; }
        catch { }
        if (presets == null) return;
        if (object.ReferenceEquals(presets, _weatherPresets) && _weatherPresetNames.Count == presets.Length) return;

        _weatherPresets = presets;
        _weatherPresetNames.Clear();
        System.Type presetType = presets.GetType().GetElementType();
        _weatherPresetNameField = presetType == null ? null : FindField(presetType, "name");
        for (int i = 0; i < presets.Length; i++)
        {
            string name = "";
            try
            {
                object preset = presets.GetValue(i);
                object value = _weatherPresetNameField == null || preset == null ? null : _weatherPresetNameField.GetValue(preset);
                name = value == null ? "" : System.Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }
            if (string.IsNullOrEmpty(name)) name = "Preset " + (i + 1);
            _weatherPresetNames.Add(name);
        }

        if (_weatherPresetNames.Count > 0 && _selectedWeatherPreset >= _weatherPresetNames.Count)
        {
            _selectedWeatherPreset = 0;
            SaveWeatherConfig();
        }
    }

    static string WeatherPresetName(int index)
    {
        if (index >= 0 && index < _weatherPresetNames.Count) return _weatherPresetNames[index];
        return index >= 0 ? "Preset " + (index + 1) : "-";
    }

    static bool ApplyWeatherPreset(int index, bool announce)
    {
        if (_weatherController == null || _weatherSetPresetMethod == null) return false;
        if (index < 0 || index >= _weatherPresetNames.Count) return false;
        try
        {
            _weatherSetPresetMethod.Invoke(_weatherController, new object[] { index });
            _weatherCurrentIndex = index;
            if (announce) _lastAction = "Погода: " + WeatherPresetName(index);
            return true;
        }
        catch (System.Exception ex)
        {
            _weatherStatus = "Ошибка применения погоды: " + ex.Message;
            if (_log != null) _log.LogWarning("[FoATrainer] SetPreset error: " + ex.Message);
            return false;
        }
    }

    static void SetWeatherOverride(bool enabled)
    {
        _weatherOverrideEnabled = enabled;
        _weatherDropdownOpen = false;
        SaveWeatherConfig();
        if (enabled)
        {
            ApplyWeatherPreset(_selectedWeatherPreset, true);
            _lastAction = "Принудительная погода: " + WeatherPresetName(_selectedWeatherPreset);
        }
        else
        {
            // Do not call ResumePrecipitation here: quests can intentionally stop rain.
            // The native controller remains active and resumes its own scheduled changes.
            _lastAction = "Погода: автоматический режим";
        }
    }

    static void SelectWeatherPreset(int index)
    {
        if (index < 0 || index >= _weatherPresetNames.Count) return;
        _selectedWeatherPreset = index;
        _weatherOverrideEnabled = true;
        _weatherDropdownOpen = false;
        SaveWeatherConfig();
        ApplyWeatherPreset(index, true);
    }

    static void MaintainWeatherControl()
    {
        if (UnityEngine.Time.unscaledTime < _weatherNextRefresh) return;
        _weatherNextRefresh = UnityEngine.Time.unscaledTime + 0.5f;
        try
        {
            if (_weatherConfig == null) InitializeWeatherConfig();
            if (_weatherController == null || IsDiscarded(_weatherController))
            {
                ResetWeatherAccessors(FindWeatherController());
            }
            if (_weatherController == null)
            {
                _weatherStatus = "Погодная система еще не загружена";
                return;
            }

            RefreshWeatherPresets();
            if (_weatherCurrentIndexField != null)
            {
                try { _weatherCurrentIndex = ToInt(_weatherCurrentIndexField.GetValue(_weatherController), -1); }
                catch { _weatherCurrentIndex = -1; }
            }
            _weatherPrecipitationIntensity = ToFloat(GetProp(_weatherController, "PrecipitationIntensity"), 0f);
            _weatherRainIntensity = ToFloat(GetProp(_weatherController, "RainIntensity"), 0f);
            _weatherSnowIntensity = ToFloat(GetProp(_weatherController, "SnowIntensity"), 0f);
            _weatherHeavyRain = ToBool(GetProp(_weatherController, "HeavyRain"));

            if (_weatherOverrideEnabled && _weatherPresetNames.Count > 0 && _weatherCurrentIndex != _selectedWeatherPreset)
            {
                ApplyWeatherPreset(_selectedWeatherPreset, false);
            }
            _weatherStatus = _weatherPresetNames.Count > 0
                ? "WeatherController: " + _weatherPresetNames.Count + " presets"
                : "WeatherController: preset list unavailable";
        }
        catch (System.Exception ex)
        {
            _weatherStatus = "WeatherController: " + ex.Message;
            if (_log != null) _log.LogWarning("[FoATrainer] Weather update error: " + ex.Message);
        }
    }

    static object GetEspProp(object obj, string name)
    {
        if (obj == null) return null;
        try
        {
            System.Type t = obj.GetType();
            string key = t.FullName + "|" + name;
            System.Reflection.MemberInfo member = null;
            if (_espMemberCache.TryGetValue(key, out member) && member != null)
            {
                System.Reflection.PropertyInfo cp = member as System.Reflection.PropertyInfo;
                if (cp != null) return cp.GetValue(obj, null);
                System.Reflection.FieldInfo cf = member as System.Reflection.FieldInfo;
                if (cf != null) return cf.GetValue(obj);
            }
            if (_espMissingMemberCache.ContainsKey(key)) return null;

            System.Reflection.PropertyInfo p = FindProperty(t, name, false);
            if (p != null)
            {
                _espMemberCache[key] = p;
                return p.GetValue(obj, null);
            }
            System.Reflection.FieldInfo f = FindField(t, name);
            if (f != null)
            {
                _espMemberCache[key] = f;
                return f.GetValue(obj);
            }
            _espMissingMemberCache[key] = true;
        }
        catch { }
        return null;
    }

    static bool IsDiscarded(object obj)
    {
        if (obj == null) return true;
        object a = GetEspProp(obj, "HasBeenDiscarded");
        if (a != null && ToBool(a)) return true;
        object b = GetEspProp(obj, "WasDiscarded");
        if (b != null && ToBool(b)) return true;
        return false;
    }

    static bool TryVector3(object value, out UnityEngine.Vector3 result)
    {
        result = UnityEngine.Vector3.zero;
        if (value == null) return false;
        if (value is UnityEngine.Vector3)
        {
            result = (UnityEngine.Vector3)value;
            return true;
        }
        return false;
    }

    static bool EspIsNpcType(EspEntityType kind)
    {
        return kind == EspEntityType.Hostile || kind == EspEntityType.Neutral || kind == EspEntityType.Friendly || kind == EspEntityType.Merchant;
    }

    static bool EspCategoryEnabled(EspEntityType kind)
    {
        if (kind == EspEntityType.Hostile) return EspEnemies;
        if (kind == EspEntityType.Friendly) return EspFriendlies;
        if (kind == EspEntityType.Merchant) return EspMerchants;
        if (kind == EspEntityType.Neutral) return EspNpcs;
        if (kind == EspEntityType.Item) return EspItems;
        return EspContainers;
    }

    static bool TryEspPosition(object obj, EspEntityType kind, out UnityEngine.Vector3 position, out UnityEngine.Transform anchor, out UnityEngine.Vector3 anchorOffset)
    {
        position = UnityEngine.Vector3.zero;
        anchor = null;
        anchorOffset = UnityEngine.Vector3.zero;
        if (obj == null) return false;
        try
        {
            if (EspIsNpcType(kind))
            {
                object head = GetEspProp(obj, "Head");
                UnityEngine.Transform headTransform = head as UnityEngine.Transform;
                if (headTransform != null)
                {
                    anchor = headTransform;
                    anchorOffset = new UnityEngine.Vector3(0f, 0.18f, 0f);
                    position = headTransform.position + anchorOffset;
                    return true;
                }
            }
            object coords = GetEspProp(obj, "Coords");
            if (TryVector3(coords, out position))
            {
                if (EspIsNpcType(kind)) position.y += 1.75f;
                else position.y += 0.30f;
                return true;
            }
            object interaction = GetEspProp(obj, "InteractionPosition");
            if (TryVector3(interaction, out position))
            {
                position.y += 0.25f;
                return true;
            }
        }
        catch { }
        return false;
    }

    static string EspName(object obj, object template, string fallback)
    {
        string value = null;
        object v = GetEspProp(obj, "DisplayName");
        if (v != null) value = System.Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture);
        if (string.IsNullOrEmpty(value))
        {
            v = GetEspProp(obj, "Name");
            if (v != null) value = System.Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture);
        }
        if (string.IsNullOrEmpty(value) && template != null)
        {
            v = GetEspProp(template, "ItemName");
            if (v != null) value = System.Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture);
        }
        if (string.IsNullOrEmpty(value)) value = fallback;
        return value;
    }

    static bool EspItemTemplateAllowed(object template)
    {
        if (template == null) return EspShowItemOther;
        bool weapon = ToBool(GetEspProp(template, "IsWeapon"));
        bool armor = ToBool(GetEspProp(template, "IsArmor")) || ToBool(GetEspProp(template, "IsShield"));
        bool consumable = ToBool(GetEspProp(template, "IsConsumable"));
        bool material = ToBool(GetEspProp(template, "IsCrafting")) || ToBool(GetEspProp(template, "IsComponent"));
        bool important = ToBool(GetEspProp(template, "IsImportantItem")) || ToBool(GetEspProp(template, "IsKey"));
        if (weapon) return EspShowItemWeapons;
        if (armor) return EspShowItemArmor;
        if (consumable) return EspShowItemConsumables;
        if (material) return EspShowItemMaterials;
        if (important) return EspShowItemImportant;
        return EspShowItemOther;
    }

    static string EspObjectId(object obj)
    {
        if (obj == null) return "";
        try
        {
            object value = GetEspProp(obj, "ID");
            if (value == null) return "";
            return System.Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch { return ""; }
    }

    static bool EspLocationIsMerchant(object location)
    {
        if (location == null) return false;
        if (_espMerchantLocations.ContainsKey(location)) return true;
        string id = EspObjectId(location);
        return !string.IsNullOrEmpty(id) && _espMerchantLocationIds.ContainsKey(id);
    }

    static EspEntityType EspClassifyNpc(object npc)
    {
        if (npc == null) return EspEntityType.Neutral;

        // A Shop is a dedicated game model attached to the same Location as its merchant NPC.
        // Merchant takes priority over the NPC's combat-capable base class.
        object location = GetEspProp(npc, "ParentModel");
        if (EspLocationIsMerchant(location)) return EspEntityType.Merchant;

        // NpcElement.AntagonismToHero is the runtime faction result used by the game:
        // Friendly = 0, Neutral = 1, Hostile = 2.
        object antagonism = GetEspProp(npc, "AntagonismToHero");
        if (antagonism != null)
        {
            int relation = ToInt(antagonism, -1);
            if (relation == 0) return EspEntityType.Friendly;
            if (relation == 1) return EspEntityType.Neutral;
            if (relation == 2) return EspEntityType.Hostile;
        }

        // Compatibility fallbacks for game versions where the relation property cannot be read.
        if (ToBool(GetEspProp(npc, "IsSummonOrAlly"))) return EspEntityType.Friendly;
        if (GetEspProp(npc, "EnemyBaseClass") != null) return EspEntityType.Hostile;
        return EspEntityType.Neutral;
    }

    static string EspNpcFallbackName(EspEntityType kind)
    {
        if (kind == EspEntityType.Merchant) return Language == 1 ? "Торговец" : "Merchant";
        if (kind == EspEntityType.Friendly) return Language == 1 ? "Союзник" : "Friendly";
        if (kind == EspEntityType.Hostile) return Language == 1 ? "Враг" : "Enemy";
        return Language == 1 ? "Нейтральный NPC" : "Neutral NPC";
    }

    static string EspItemIcon(object template)
    {
        if (template == null) return "I";
        if (ToBool(GetEspProp(template, "IsWeapon"))) return "W";
        if (ToBool(GetEspProp(template, "IsArmor")) || ToBool(GetEspProp(template, "IsShield"))) return "A";
        if (ToBool(GetEspProp(template, "IsConsumable"))) return "+";
        if (ToBool(GetEspProp(template, "IsCrafting")) || ToBool(GetEspProp(template, "IsComponent"))) return "M";
        if (ToBool(GetEspProp(template, "IsImportantItem")) || ToBool(GetEspProp(template, "IsKey"))) return "*";
        return "I";
    }

    static bool EspCallIsEmpty(object searchAction)
    {
        if (searchAction == null) return false;
        try
        {
            System.Reflection.MethodInfo m = FindMethod(searchAction.GetType(), "IsEmpty", 0);
            if (m == null) return false;
            object value = m.Invoke(searchAction, null);
            return value != null && ToBool(value);
        }
        catch { return false; }
    }

    static bool EspWithinDistance(object source, EspEntityType kind, UnityEngine.Vector3 heroPos, float maxDistance)
    {
        if (source == null) return false;
        UnityEngine.Vector3 pos;
        UnityEngine.Transform anchor;
        UnityEngine.Vector3 anchorOffset;
        if (!TryEspPosition(source, kind, out pos, out anchor, out anchorOffset)) return false;
        UnityEngine.Vector3 delta = pos - heroPos;
        return delta.sqrMagnitude <= maxDistance * maxDistance;
    }

    static string EspHealthText(object npc, out float ratio)
    {
        ratio = -1f;
        if (npc == null) return "";
        object health = GetEspProp(npc, "Health");
        object maxHealth = GetEspProp(npc, "MaxHealth");
        if (health == null) return "";
        float current = StatModified(health);
        float max = maxHealth != null ? StatModified(maxHealth) : StatUpper(health);
        if (max <= 0.001f) max = StatUpper(health);
        if (max > 0.001f) ratio = Clamp(current / max, 0f, 1f);
        return System.Math.Round(current).ToString(System.Globalization.CultureInfo.InvariantCulture) + "/" + System.Math.Round(max).ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    static void AddEspEntry(object source, object template, EspEntityType kind, string fallback, UnityEngine.Vector3 heroPos, float maxDistance)
    {
        if (source == null || IsDiscarded(source)) return;
        UnityEngine.Vector3 pos;
        UnityEngine.Transform anchor;
        UnityEngine.Vector3 anchorOffset;
        if (!TryEspPosition(source, kind, out pos, out anchor, out anchorOffset)) return;
        float distance = UnityEngine.Vector3.Distance(heroPos, pos);
        if (distance > maxDistance) return;
        EspEntry entry = new EspEntry();
        entry.Source = source;
        entry.Template = template;
        entry.Anchor = anchor;
        entry.AnchorOffset = anchorOffset;
        entry.Position = pos;
        entry.Kind = kind;
        entry.Name = EspName(source, template, fallback);
        entry.Distance = distance;
        entry.HealthRatio = -1f;
        entry.HealthText = "";
        entry.StateText = "";
        entry.IsDead = false;
        entry.IsEmpty = false;
        if (kind == EspEntityType.Item) entry.IconText = EspItemIcon(template);
        else if (kind == EspEntityType.Container) entry.IconText = "C";
        else if (kind == EspEntityType.Hostile) entry.IconText = "!";
        else if (kind == EspEntityType.Friendly) entry.IconText = "+";
        else if (kind == EspEntityType.Merchant) entry.IconText = "$";
        else entry.IconText = "N";
        if ((EspShowHealth || EspShowHealthBars) && EspIsNpcType(kind))
            entry.HealthText = EspHealthText(source, out entry.HealthRatio);
        _espEntries.Add(entry);
        if (kind == EspEntityType.Item) _espItemsCached++;
        else if (kind == EspEntityType.Container) _espContainersCached++;
        else if (kind == EspEntityType.Hostile) _espEnemiesCached++;
        else if (kind == EspEntityType.Neutral) _espNpcsCached++;
        else if (kind == EspEntityType.Friendly) _espFriendliesCached++;
        else if (kind == EspEntityType.Merchant) _espMerchantsCached++;
    }

    static void AddEspLootEntry(object searchAction, object location, int npcState, UnityEngine.Vector3 heroPos)
    {
        if (searchAction == null || location == null || IsDiscarded(location)) return;
        bool corpse = npcState < 0;
        EspEntityType kind = corpse ? (EspEntityType)(-npcState) : EspEntityType.Container;
        if (corpse && !EspShowDead) return;
        if (corpse && !EspCategoryEnabled(kind)) return;
        if (!corpse && !EspContainers) return;

        float maxDistance = corpse ? (kind == EspEntityType.Hostile ? EspEnemyDistance : EspNpcDistance) : EspContainerDistance;
        if (!EspWithinDistance(location, kind, heroPos, maxDistance)) return;

        bool empty = false;
        if (EspShowLootState || EspHideEmptyLoot) empty = EspCallIsEmpty(searchAction);
        if (EspHideEmptyLoot && empty) return;

        int before = _espEntries.Count;
        AddEspEntry(location, null, kind, corpse ? (Language == 1 ? "Труп" : "Corpse") : (Language == 1 ? "Контейнер" : "Container"), heroPos, maxDistance);
        if (_espEntries.Count <= before) return;
        EspEntry entry = _espEntries[_espEntries.Count - 1];
        entry.IsDead = corpse;
        entry.IsEmpty = empty;
        entry.IconText = corpse ? "X" : "C";
        if (EspShowLootState) entry.StateText = empty ? (Language == 1 ? "ПУСТО" : "EMPTY") : (Language == 1 ? "ЛУТ" : "LOOT");
        if (corpse) _espCorpsesCached++;
    }

    static void ScanEspMerchantLocations(System.Collections.IEnumerable enumerable)
    {
        if (enumerable == null) return;
        System.IDisposable disposable = enumerable as System.IDisposable;
        try
        {
            System.Collections.IEnumerator it = enumerable.GetEnumerator();
            while (it.MoveNext())
            {
                object shop = it.Current;
                if (shop == null || IsDiscarded(shop)) continue;
                object location = GetEspProp(shop, "ParentModel");
                if (location == null) continue;
                _espMerchantLocations[location] = true;
                string id = EspObjectId(location);
                if (!string.IsNullOrEmpty(id)) _espMerchantLocationIds[id] = true;
            }
        }
        catch (System.Exception ex)
        {
            _espStatus = "ESP merchant scan error: " + ex.Message;
        }
        finally
        {
            if (disposable != null)
            {
                try { disposable.Dispose(); } catch { }
            }
        }
    }

    static void ScanEspEnumerable(System.Collections.IEnumerable enumerable, int mode, UnityEngine.Vector3 heroPos)
    {
        if (enumerable == null) return;
        System.IDisposable disposable = enumerable as System.IDisposable;
        try
        {
            System.Collections.IEnumerator it = enumerable.GetEnumerator();
            while (it.MoveNext())
            {
                object obj = it.Current;
                if (obj == null || IsDiscarded(obj)) continue;
                if (mode == 1) _espItemsRaw++;
                else if (mode == 2) _espContainersRaw++;
                else if (mode == 3) _espNpcsRaw++;
                if (mode == 1)
                {
                    object location = GetEspProp(obj, "ParentModel");
                    if (location == null) continue;
                    object spawningData = null;
                    try
                    {
                        if (_espPickItemDataField == null) _espPickItemDataField = FindField(obj.GetType(), "_itemSpawningData");
                        if (_espPickItemDataField != null) spawningData = _espPickItemDataField.GetValue(obj);
                    }
                    catch { }
                    object template = GetEspProp(spawningData, "ItemTemplate");
                    if (!EspItemTemplateAllowed(template)) continue;
                    AddEspEntry(location, template, EspEntityType.Item, Language == 1 ? "Предмет" : "Item", heroPos, EspItemDistance);
                }
                else if (mode == 2)
                {
                    // SearchAction is the actual searchable container/corpse interaction.
                    bool available = ToBool(GetEspProp(obj, "SearchAvailable"));
                    if (!available) continue;
                    object location = GetEspProp(obj, "ParentModel");
                    if (location == null) continue;
                    string id = EspObjectId(location);
                    int npcState = 0;
                    if (!string.IsNullOrEmpty(id)) _espNpcLocationState.TryGetValue(id, out npcState);
                    // SearchAction on an NPC is a corpse interaction. Living NPC search actions are skipped.
                    if (npcState > 0) continue;
                    AddEspLootEntry(obj, location, npcState, heroPos);
                }
                else if (mode == 3)
                {
                    if (ToBool(GetEspProp(obj, "IsDisappeared"))) continue;
                    bool alive = ToBool(GetEspProp(obj, "IsAlive"));
                    EspEntityType category = EspClassifyNpc(obj);
                    object location = GetEspProp(obj, "ParentModel");
                    string id = EspObjectId(location);
                    if (!string.IsNullOrEmpty(id)) _espNpcLocationState[id] = alive ? (int)category : -(int)category;
                    // Dead NPCs are drawn only from their SearchAction, so they cannot be duplicated as living NPCs.
                    if (!alive) continue;
                    if (!EspCategoryEnabled(category)) continue;
                    float maxDistance = category == EspEntityType.Hostile ? EspEnemyDistance : EspNpcDistance;
                    AddEspEntry(obj, null, category, EspNpcFallbackName(category), heroPos, maxDistance);
                }
            }
        }
        catch (System.Exception ex)
        {
            _espStatus = "ESP enumerate error: " + ex.Message;
        }
        finally
        {
            if (disposable != null)
            {
                try { disposable.Dispose(); } catch { }
            }
        }
    }

    static void UpdateEspCache(object hero)
    {
        float now = UnityEngine.Time.realtimeSinceStartup;
        // Do not allow old profiles to force an aggressive 0.1-0.35s full scan.
        float interval = Clamp(EspScanInterval, 0.50f, 3.0f);
        if (now < _espNextScan) return;
        _espNextScan = now + interval;
        UnityEngine.Vector3 heroPos;
        if (!TryVector3(GetEspProp(hero, "Coords"), out heroPos)) return;
        _espEntries.Clear();
        _espNpcLocationState.Clear();
        _espMerchantLocations.Clear();
        _espMerchantLocationIds.Clear();
        _espItemsCached = 0;
        _espContainersCached = 0;
        _espEnemiesCached = 0;
        _espNpcsCached = 0;
        _espFriendliesCached = 0;
        _espMerchantsCached = 0;
        _espCorpsesCached = 0;
        _espItemsRaw = 0;
        _espContainersRaw = 0;
        _espNpcsRaw = 0;
        // Shop pass builds the merchant Location set. NPC pass then uses it before faction classification.
        if (EspMerchants || EspEnemies || EspFriendlies || EspNpcs || EspShowDead)
            ScanEspMerchantLocations(WorldAll("Awaken.TG.Main.Locations.Shops.Shop"));
        // NPC pass builds a Location ID state map used by SearchAction corpse detection.
        if (EspEnemies || EspFriendlies || EspMerchants || EspNpcs || EspContainers || EspShowDead)
            ScanEspEnumerable(WorldAll("Awaken.TG.Main.Fights.NPCs.NpcElement"), 3, heroPos);
        if (EspItems) ScanEspEnumerable(WorldAll("Awaken.TG.Main.Locations.Actions.PickItemAction"), 1, heroPos);
        if (EspContainers || (EspShowDead && (EspEnemies || EspFriendlies || EspMerchants || EspNpcs)))
            ScanEspEnumerable(WorldAll("Awaken.TG.Main.Locations.Actions.SearchAction"), 2, heroPos);
        _espEntries.Sort(delegate(EspEntry a, EspEntry b) { return a.Distance.CompareTo(b.Distance); });
        _espStatus = (Language == 1 ? "Кеш: " : "Cache: ") + _espEntries.Count +
            " | I " + _espItemsCached + "/" + _espItemsRaw +
            " C " + _espContainersCached + "/" + _espContainersRaw +
            " H " + _espEnemiesCached + " F " + _espFriendliesCached + " N " + _espNpcsCached + " M " + _espMerchantsCached +
            " D " + _espCorpsesCached + "/" + _espNpcsRaw;
    }

    static UnityEngine.Vector3 EspEntryPosition(EspEntry entry)
    {
        if (entry == null) return UnityEngine.Vector3.zero;
        if (entry.Anchor != null) return entry.Anchor.position + entry.AnchorOffset;
        return entry.Position;
    }

    static UnityEngine.Camera ResolveEspCamera()
    {
        if (_espCamera != null && _espCamera.enabled && _espCamera.gameObject != null && _espCamera.gameObject.activeInHierarchy)
            return _espCamera;
        float now = UnityEngine.Time.realtimeSinceStartup;
        if (now < _espNextCameraResolve) return null;
        _espNextCameraResolve = now + 1.0f;
        _espCamera = null;
        try
        {
            UnityEngine.Camera main = UnityEngine.Camera.main;
            if (main != null && main.enabled && main.gameObject.activeInHierarchy) _espCamera = main;
            if (_espCamera == null)
            {
                UnityEngine.Camera[] cameras = UnityEngine.Camera.allCameras;
                float bestScore = -1000000f;
                for (int i = 0; i < cameras.Length; i++)
                {
                    UnityEngine.Camera c = cameras[i];
                    if (c == null || !c.enabled || c.gameObject == null || !c.gameObject.activeInHierarchy) continue;
                    if (c.targetTexture != null) continue;
                    float score = c.depth * 1000f + c.pixelWidth * c.pixelHeight * 0.0001f;
                    if (c.cameraType == UnityEngine.CameraType.Game) score += 100000f;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        _espCamera = c;
                    }
                }
            }
            _espCameraName = _espCamera != null ? _espCamera.name : "NONE";
        }
        catch (System.Exception ex)
        {
            _espCameraName = "ERR";
            _espStatus = "ESP camera error: " + ex.Message;
        }
        return _espCamera;
    }

    static UnityEngine.Color EspColorForKind(EspEntityType kind)
    {
        if (kind == EspEntityType.Item) return new UnityEngine.Color(0.35f, 0.86f, 1.00f, 1f);
        if (kind == EspEntityType.Container) return new UnityEngine.Color(1.00f, 0.72f, 0.23f, 1f);
        if (kind == EspEntityType.Hostile) return new UnityEngine.Color(1.00f, 0.30f, 0.27f, 1f);
        if (kind == EspEntityType.Friendly) return new UnityEngine.Color(0.35f, 0.95f, 0.50f, 1f);
        if (kind == EspEntityType.Merchant) return new UnityEngine.Color(0.78f, 0.55f, 1.00f, 1f);
        return new UnityEngine.Color(1.00f, 0.86f, 0.35f, 1f);
    }

    static UnityEngine.Color EspColorForEntry(EspEntry entry)
    {
        if (entry == null) return UnityEngine.Color.white;
        if (entry.IsDead || entry.IsEmpty) return new UnityEngine.Color(0.62f, 0.64f, 0.68f, 1f);
        return EspColorForKind(entry.Kind);
    }

    static float EspDistanceForEntry(EspEntry entry, UnityEngine.Vector3 heroPos, UnityEngine.Vector3 worldPos)
    {
        if (entry == null) return 0f;
        return UnityEngine.Vector3.Distance(heroPos, worldPos);
    }

    static float EspMaxDistanceForKind(EspEntityType kind)
    {
        if (kind == EspEntityType.Item) return EspItemDistance;
        if (kind == EspEntityType.Container) return EspContainerDistance;
        if (kind == EspEntityType.Hostile) return EspEnemyDistance;
        return EspNpcDistance;
    }

    static void DrawEspRect(UnityEngine.Rect rect, UnityEngine.Color color)
    {
        UnityEngine.Color old = UnityEngine.GUI.color;
        UnityEngine.GUI.color = color;
        UnityEngine.GUI.DrawTexture(rect, UnityEngine.Texture2D.whiteTexture);
        UnityEngine.GUI.color = old;
    }

    static void DrawEspOverlay()
    {
        _espVisibleLastFrame = 0;
        if (!EspEnabled) return;
        if (_espTextStyle == null) return;
        UnityEngine.Camera camera = ResolveEspCamera();
        string hud = (Language == 1 ? "ESP ВКЛ" : "ESP ON") + "  |  " + (_espEntries == null ? 0 : _espEntries.Count) + "  |  " + _espCameraName;
        _espTextStyle.fontSize = 12;
        _espTextStyle.normal.textColor = new UnityEngine.Color(1.00f, 0.62f, 0.18f, 1f);
        UnityEngine.Rect hudRect = new UnityEngine.Rect(14f, 14f, 260f, 24f);
        DrawEspRect(hudRect, new UnityEngine.Color(0.02f, 0.025f, 0.035f, 0.78f));
        UnityEngine.GUI.Label(hudRect, hud, _espTextStyle);
        if (_espEntries == null || _espEntries.Count == 0) return;
        object hero = Hero();
        if (hero == null) return;
        if (camera == null)
        {
            _espStatus = Language == 1 ? "ESP: игровая камера не найдена" : "ESP: gameplay camera not found";
            return;
        }
        UnityEngine.Vector3 heroPos;
        if (!TryVector3(GetProp(hero, "Coords"), out heroPos)) return;
        if (_espTextStyle == null) return;
        int maxLabels = EspMaxObjects;
        if (maxLabels < 10) maxLabels = 10;
        if (maxLabels > 300) maxLabels = 300;
        int drawn = 0;
        for (int i = 0; i < _espEntries.Count && drawn < maxLabels; i++)
        {
            EspEntry entry = _espEntries[i];
            if (entry == null) continue;
            UnityEngine.Vector3 worldPos = EspEntryPosition(entry);
            float distance = UnityEngine.Vector3.Distance(heroPos, worldPos);
            if (distance > EspMaxDistanceForKind(entry.Kind)) continue;
            UnityEngine.Vector3 sp = camera.WorldToScreenPoint(worldPos);
            if (sp.z <= 0.05f) continue;
            float x = sp.x;
            float y = UnityEngine.Screen.height - sp.y;
            if (x < -80f || x > UnityEngine.Screen.width + 80f || y < -60f || y > UnityEngine.Screen.height + 60f) continue;

            string text = "";
            if (!EspIconsOnly && EspShowNames) text = entry.Name;
            if (EspShowDistance)
            {
                string dist = System.Math.Round(distance).ToString(System.Globalization.CultureInfo.InvariantCulture) + "m";
                text = text.Length > 0 ? text + "  [" + dist + "]" : dist;
            }
            float hpRatio = entry.HealthRatio;
            if (EspShowHealth && EspIsNpcType(entry.Kind) && !entry.IsDead && !string.IsNullOrEmpty(entry.HealthText))
                text = text.Length > 0 ? text + "  HP " + entry.HealthText : "HP " + entry.HealthText;
            if (EspShowLootState && !string.IsNullOrEmpty(entry.StateText))
                text = text.Length > 0 ? text + "  [" + entry.StateText + "]" : entry.StateText;

            int iconSize = EspIconSize;
            if (iconSize < 6) iconSize = 6;
            if (iconSize > 42) iconSize = 42;
            bool drawIcon = EspShowIcons && !string.IsNullOrEmpty(entry.IconText);
            bool drawBar = EspShowHealthBars && !entry.IsDead && hpRatio >= 0f && EspIsNpcType(entry.Kind);
            if (!drawIcon && text.Length == 0 && !drawBar) continue;

            UnityEngine.Color entryColor = EspColorForEntry(entry);
            float labelY = y;
            if (drawIcon)
            {
                UnityEngine.Rect badge = new UnityEngine.Rect(x - iconSize * 0.5f, y - iconSize * 0.5f, iconSize, iconSize);
                DrawEspRect(badge, new UnityEngine.Color(0.02f, 0.025f, 0.035f, 0.82f));
                int oldSize = _espTextStyle.fontSize;
                UnityEngine.TextAnchor oldAlign = _espTextStyle.alignment;
                UnityEngine.Color oldColor = _espTextStyle.normal.textColor;
                _espTextStyle.fontSize = iconSize > 18 ? iconSize - 8 : System.Math.Max(4, iconSize - 2);
                _espTextStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
                _espTextStyle.normal.textColor = entryColor;
                UnityEngine.GUI.Label(badge, entry.IconText, _espTextStyle);
                _espTextStyle.fontSize = oldSize;
                _espTextStyle.alignment = oldAlign;
                _espTextStyle.normal.textColor = oldColor;
                labelY = badge.yMax + 2f;
            }

            UnityEngine.Rect labelRect = new UnityEngine.Rect(x, labelY, 0f, 0f);
            if (text.Length > 0)
            {
                _espTextStyle.fontSize = EspFontSize < 5 ? 5 : (EspFontSize > 24 ? 24 : EspFontSize);
                _espTextStyle.normal.textColor = entryColor;
                UnityEngine.GUIContent gc = new UnityEngine.GUIContent(text);
                UnityEngine.Vector2 size = _espTextStyle.CalcSize(gc);
                float horizontalPadding = UnityEngine.Mathf.Max(4f, _espTextStyle.fontSize * 0.75f);
                float verticalPadding = UnityEngine.Mathf.Max(2f, _espTextStyle.fontSize * 0.40f);
                float w = size.x + horizontalPadding;
                float h = size.y + verticalPadding;
                labelRect = new UnityEngine.Rect(x - w * 0.5f, labelY, w, h);
                if (EspShowBackground)
                    DrawEspRect(labelRect, new UnityEngine.Color(0.02f, 0.025f, 0.035f, 0.72f));
                UnityEngine.GUI.Label(labelRect, text, _espTextStyle);
                labelY = labelRect.yMax + 2f;
            }

            if (drawBar)
            {
                int barWidth = EspHealthBarWidth;
                if (barWidth < 8) barWidth = 8;
                if (barWidth > 120) barWidth = 120;
                int barHeight = EspHealthBarHeight;
                if (barHeight < 1) barHeight = 1;
                if (barHeight > 8) barHeight = 8;
                UnityEngine.Rect barBg = new UnityEngine.Rect(x - barWidth * 0.5f, labelY, barWidth, barHeight);
                DrawEspRect(barBg, new UnityEngine.Color(0.12f, 0.12f, 0.12f, 0.88f));
                UnityEngine.Color hpColor = hpRatio > 0.55f ? new UnityEngine.Color(0.25f, 0.90f, 0.35f, 0.95f) : (hpRatio > 0.25f ? new UnityEngine.Color(1.00f, 0.70f, 0.18f, 0.95f) : new UnityEngine.Color(1.00f, 0.24f, 0.20f, 0.95f));
                DrawEspRect(new UnityEngine.Rect(barBg.x, barBg.y, barBg.width * hpRatio, barBg.height), hpColor);
            }
            drawn++;
        }
        _espVisibleLastFrame = drawn;
    }

    static void MaintainCheats(object hero)
    {
        if (InfiniteHealth) FillStat(GetProp(hero, "Health"));
        if (InfiniteMana) FillStat(GetProp(hero, "Mana"));
        if (InfiniteStamina) FillStat(GetProp(hero, "Stamina"));
        if (InfiniteKingsPower) FillStat(GetProp(hero, "WyrdSkillDuration"));
        if (InfiniteOxygen) FillStat(GetProp(HeroStats(hero), "OxygenLevel"));
        if (StealthMode) ApplyStealth(hero);
        if (EasyLockPicking) ApplyEasyLock(hero);
        if (ManaRateEnabled) ApplyManaRate(hero);
        if (StaminaRateEnabled) ApplyStaminaRate(hero);
        if (MovementSpeedEnabled) ApplyMovementSpeed(hero);
        if (JumpHeightEnabled) ApplyJumpHeight(hero);
        if (FlightEnabled) MaintainFlight(hero);
        if (GameSpeedEnabled && UnityEngine.Time.timeScale > 0.0001f) UnityEngine.Time.timeScale = Clamp(GameSpeed, 0.05f, 20f);
    }

    static float Clamp(float v, float min, float max)
    {
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }

    static void ResetSavedTemporaries()
    {
        _stealthSaved = false;
        _lockSaved = false;
        _manaRateSaved = false;
        _staminaRateSaved = false;
        _movementSaved = false;
        _jumpSaved = false;
        _flightControllerSaved = false;
        _flightController = null;
    }

    static void RestoreAllTemporaryStats()
    {
        object hero = Hero();
        if (hero != null)
        {
            RestoreStealth(hero);
            RestoreEasyLock(hero);
            RestoreManaRate(hero);
            RestoreStaminaRate(hero);
            RestoreMovementSpeed(hero);
            RestoreJumpHeight(hero);
            RestoreFlightController();
        }
        if (_gameSpeedSaved)
        {
            UnityEngine.Time.timeScale = _gameSpeedOrig;
            _gameSpeedSaved = false;
        }
    }

    static void SetStealth(bool value)
    {
        if (StealthMode == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreStealth(hero);
        StealthMode = value;
        if (value) { _stealthSaved = false; if (hero != null) ApplyStealth(hero); }
    }

    static void ApplyStealth(object hero)
    {
        object hs = HeroStats(hero);
        if (hs == null) return;
        object a = GetProp(hs, "VisibilityMultiplier");
        object b = GetProp(hs, "CrouchVisibilityMultiplier");
        object c = GetProp(hs, "NoiseMultiplier");
        object d = GetProp(hs, "CrouchNoiseMultiplier");
        if (!_stealthSaved)
        {
            _visOrig = StatBase(a); _crouchVisOrig = StatBase(b); _noiseOrig = StatBase(c); _crouchNoiseOrig = StatBase(d);
            _stealthSaved = true;
        }
        SetStatRaw(a, 0f); SetStatRaw(b, 0f); SetStatRaw(c, 0f); SetStatRaw(d, 0f);
    }

    static void RestoreStealth(object hero)
    {
        if (!_stealthSaved) return;
        object hs = HeroStats(hero);
        SetStatRaw(GetProp(hs, "VisibilityMultiplier"), _visOrig);
        SetStatRaw(GetProp(hs, "CrouchVisibilityMultiplier"), _crouchVisOrig);
        SetStatRaw(GetProp(hs, "NoiseMultiplier"), _noiseOrig);
        SetStatRaw(GetProp(hs, "CrouchNoiseMultiplier"), _crouchNoiseOrig);
        _stealthSaved = false;
    }

    static void SetEasyLock(bool value)
    {
        if (EasyLockPicking == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreEasyLock(hero);
        EasyLockPicking = value;
        if (value) { _lockSaved = false; if (hero != null) ApplyEasyLock(hero); }
    }

    static void ApplyEasyLock(object hero)
    {
        object hs = HeroStats(hero);
        object damage = GetProp(hs, "LockpickDamageMultiplier");
        object tolerance = GetProp(hs, "LockpickToleranceMultiplier");
        if (!_lockSaved)
        {
            _lockDamageOrig = StatBase(damage); _lockToleranceOrig = StatBase(tolerance); _lockSaved = true;
        }
        SetStatRaw(damage, 0f);
        SetStatRaw(tolerance, 100f);
    }

    static void RestoreEasyLock(object hero)
    {
        if (!_lockSaved) return;
        object hs = HeroStats(hero);
        SetStatRaw(GetProp(hs, "LockpickDamageMultiplier"), _lockDamageOrig);
        SetStatRaw(GetProp(hs, "LockpickToleranceMultiplier"), _lockToleranceOrig);
        _lockSaved = false;
    }

    static void SetManaRate(bool value)
    {
        if (ManaRateEnabled == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreManaRate(hero);
        ManaRateEnabled = value;
        if (value) { _manaRateSaved = false; if (hero != null) ApplyManaRate(hero); }
    }

    static void ApplyManaRate(object hero)
    {
        object stat = GetProp(CharacterStats(hero), "ManaUsageMultiplier");
        if (!_manaRateSaved) { _manaRateOrig = StatBase(stat); _manaRateSaved = true; }
        SetStatRaw(stat, ManaRate);
    }

    static void RestoreManaRate(object hero)
    {
        if (!_manaRateSaved) return;
        SetStatRaw(GetProp(CharacterStats(hero), "ManaUsageMultiplier"), _manaRateOrig);
        _manaRateSaved = false;
    }

    static void SetStaminaRate(bool value)
    {
        if (StaminaRateEnabled == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreStaminaRate(hero);
        StaminaRateEnabled = value;
        if (value) { _staminaRateSaved = false; if (hero != null) ApplyStaminaRate(hero); }
    }

    static void ApplyStaminaRate(object hero)
    {
        object stat = GetProp(CharacterStats(hero), "StaminaUsageMultiplier");
        if (!_staminaRateSaved) { _staminaRateOrig = StatBase(stat); _staminaRateSaved = true; }
        SetStatRaw(stat, StaminaRate);
    }

    static void RestoreStaminaRate(object hero)
    {
        if (!_staminaRateSaved) return;
        SetStatRaw(GetProp(CharacterStats(hero), "StaminaUsageMultiplier"), _staminaRateOrig);
        _staminaRateSaved = false;
    }

    static void SetMovementSpeed(bool value)
    {
        if (MovementSpeedEnabled == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreMovementSpeed(hero);
        MovementSpeedEnabled = value;
        if (value) { _movementSaved = false; if (hero != null) ApplyMovementSpeed(hero); }
    }

    static void ApplyMovementSpeed(object hero)
    {
        object stat = GetProp(CharacterStats(hero), "MovementSpeedMultiplier");
        if (!_movementSaved) { _movementOrig = StatBase(stat); _movementSaved = true; }
        SetStatRaw(stat, _movementOrig * Clamp(MovementSpeed, 0.05f, 20f));
    }

    static void RestoreMovementSpeed(object hero)
    {
        if (!_movementSaved) return;
        SetStatRaw(GetProp(CharacterStats(hero), "MovementSpeedMultiplier"), _movementOrig);
        _movementSaved = false;
    }

    static void SetJumpHeight(bool value)
    {
        if (JumpHeightEnabled == value) return;
        object hero = Hero();
        if (!value && hero != null) RestoreJumpHeight(hero);
        JumpHeightEnabled = value;
        if (value) { _jumpSaved = false; if (hero != null) ApplyJumpHeight(hero); }
    }

    static void ApplyJumpHeight(object hero)
    {
        object stat = GetProp(HeroStats(hero), "JumpHeight");
        if (!_jumpSaved) { _jumpOrig = StatBase(stat); _jumpSaved = true; }
        SetStatRaw(stat, _jumpOrig * Clamp(JumpHeight, 0.05f, 20f));
    }

    static void RestoreJumpHeight(object hero)
    {
        if (!_jumpSaved) return;
        SetStatRaw(GetProp(HeroStats(hero), "JumpHeight"), _jumpOrig);
        _jumpSaved = false;
    }

    static void SetGameSpeed(bool value)
    {
        if (GameSpeedEnabled == value) return;
        if (value)
        {
            _gameSpeedOrig = UnityEngine.Time.timeScale;
            _gameSpeedSaved = true;
            GameSpeedEnabled = true;
        }
        else
        {
            GameSpeedEnabled = false;
            if (_gameSpeedSaved)
            {
                UnityEngine.Time.timeScale = _gameSpeedOrig;
                _gameSpeedSaved = false;
            }
        }
    }


    // ============================ V8: Flight ============================
    static void SetFlight(bool value)
    {
        if (FlightEnabled == value) return;
        if (value)
        {
            object hero = Hero();
            if (hero == null) { _lastAction = "Полет: игрок не найден"; return; }
            FlightEnabled = true;
            PrepareFlightController(hero);
            _lastAction = "Полет включен";
        }
        else
        {
            FlightEnabled = false;
            RestoreFlightController();
            object hero = Hero();
            if (hero != null)
            {
                object vhc = GetProp(hero, "VHeroController");
                System.Reflection.MethodInfo sv = vhc == null ? null : FindMethod(vhc.GetType(), "SetVerticalVelocity", 1);
                if (sv != null) { try { sv.Invoke(vhc, new object[] { 0f }); } catch { } }
            }
            _lastAction = "Полет выключен";
        }
    }

    static void PrepareFlightController(object hero)
    {
        if (hero == null) return;
        object vhc = GetProp(hero, "VHeroController");
        if (vhc == null) return;
        object controller = GetProp(vhc, "Controller");
        if (controller == null) return;
        if (!_flightControllerSaved || !object.ReferenceEquals(_flightController, controller))
        {
            _flightController = controller;
            _flightControllerWasEnabled = ToBool(GetProp(controller, "enabled"));
            _flightControllerSaved = true;
        }
        try
        {
            System.Reflection.PropertyInfo p = FindProperty(controller.GetType(), "enabled", true);
            if (p != null) p.SetValue(controller, false, null);
        }
        catch { }
        System.Reflection.MethodInfo sv = FindMethod(vhc.GetType(), "SetVerticalVelocity", 1);
        if (sv != null) { try { sv.Invoke(vhc, new object[] { 0f }); } catch { } }
    }

    static void RestoreFlightController()
    {
        if (!_flightControllerSaved || _flightController == null) return;
        try
        {
            System.Reflection.PropertyInfo p = FindProperty(_flightController.GetType(), "enabled", true);
            if (p != null) p.SetValue(_flightController, _flightControllerWasEnabled, null);
        }
        catch { }
        _flightControllerSaved = false;
        _flightController = null;
    }

    static void MaintainFlight(object hero)
    {
        if (hero == null) return;
        PrepareFlightController(hero);
        object vhc = GetProp(hero, "VHeroController");
        UnityEngine.Transform tr = null;
        if (vhc != null) tr = GetProp(vhc, "Transform") as UnityEngine.Transform;
        if (tr == null) tr = GetProp(hero, "ActorTransform") as UnityEngine.Transform;
        if (tr == null) return;

        UnityEngine.Camera cam = UnityEngine.Camera.main;
        UnityEngine.Vector3 forward = cam != null ? cam.transform.forward : tr.forward;
        UnityEngine.Vector3 right = cam != null ? cam.transform.right : tr.right;
        UnityEngine.Vector3 dir = UnityEngine.Vector3.zero;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W)) dir += forward;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S)) dir -= forward;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D)) dir += right;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A)) dir -= right;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Space)) dir += UnityEngine.Vector3.up;
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftControl) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightControl)) dir -= UnityEngine.Vector3.up;
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        float speed = Clamp(FlightSpeed, 0.1f, 200f);
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift)) speed *= Clamp(FlightBoost, 1f, 20f);
        tr.position += dir * speed * UnityEngine.Time.unscaledDeltaTime;

        if (vhc != null)
        {
            System.Reflection.MethodInfo sv = FindMethod(vhc.GetType(), "SetVerticalVelocity", 1);
            if (sv != null) { try { sv.Invoke(vhc, new object[] { 0f }); } catch { } }
        }
    }

    // ============================ V8: Disable all ============================
    static void DisableAllFunctions()
    {
        DisableAllFunctions(true);
    }

    static void DisableAllFunctions(bool includeWeather)
    {
        SetStealth(false);
        SetEasyLock(false);
        SetManaRate(false);
        SetStaminaRate(false);
        SetMovementSpeed(false);
        SetJumpHeight(false);
        SetGameSpeed(false);
        SetFlight(false);

        GodMode = false;
        InfiniteHealth = false;
        InfiniteMana = false;
        InfiniteStamina = false;
        InfiniteKingsPower = false;
        InfiniteOxygen = false;
        ItemsWontDecrease = false;
        OneHitKills = false;
        DamageMultiplierEnabled = false;
        DefenseMultiplierEnabled = false;
        ZeroItemWeight = false;
        ZeroEquipmentWeight = false;
        IgnoreCraftingRequirement = false;
        InfiniteExp = false;
        ExpMultiplierEnabled = false;
        InfiniteProfExp = false;
        ProfExpMultiplierEnabled = false;
        NoFallDamage = false;
        FreezeDaytime = false;
        TimePassSpeedEnabled = false;
        EspEnabled = false;
        _espEntries.Clear();
        if (includeWeather) SetWeatherOverride(false);
        _lastAction = "Все переключаемые функции отключены";
    }

    static System.Collections.Generic.Dictionary<string, string> _englishText;

    static void EnsureLocalization()
    {
        if (_englishText != null) return;
        System.Collections.Generic.Dictionary<string, string> d = new System.Collections.Generic.Dictionary<string, string>();
        d["Активно: "] = "Active: ";
        d["СБРОС"] = "RESET";
        d["ОКНО"] = "WINDOW";
        d["ЭКРАН"] = "FULL";
        d["ВЫКЛ ВСЕ"] = "DISABLE ALL";
        d["ИГРОК"] = "PLAYER";
        d["ИНВЕНТАРЬ"] = "INVENTORY";
        d["ОПЫТ / ВРЕМЯ"] = "XP / TIME";
        d["СТАТЫ"] = "STATS";
        d["НАСТРОЙКИ"] = "SETTINGS";
        d["ИГРОК НЕ НАЙДЕН  |  загрузите сохранение"] = "PLAYER NOT FOUND  |  load a save";
        d["ИГРОК НАЙДЕН  |  Последнее действие: "] = "PLAYER FOUND  |  Last action: ";
        d["Insert / F8 - скрыть/показать  |  F6 - ESP  |  тяните правый нижний угол для изменения размера"] = "Insert / F8 - hide/show  |  F6 - ESP  |  drag lower-right corner to resize";
        d["ESP"] = "ESP";
        d["Общий ESP"] = "Master ESP";
        d["ESP продолжает отображаться, когда меню трейнера скрыто."] = "ESP remains visible when the trainer menu is hidden.";
        d["Объекты ESP"] = "ESP objects";
        d["Предметы"] = "Items";
        d["Контейнеры"] = "Containers";
        d["Враги"] = "Enemies";
        d["NPC"] = "NPC";
        d["Враждебные NPC / враги"] = "Hostile NPCs / enemies";
        d["Дружественные NPC / союзники"] = "Friendly NPCs / allies";
        d["Нейтральные NPC"] = "Neutral NPCs";
        d["Торговцы"] = "Merchants";
        d["Показывать мертвых NPC / врагов"] = "Show dead NPCs / enemies";
        d["Фильтр предметов ESP"] = "ESP item filter";
        d["Оружие"] = "Weapons";
        d["Броня и щиты"] = "Armor and shields";
        d["Расходники"] = "Consumables";
        d["Материалы"] = "Materials";
        d["Важные / ключевые предметы"] = "Important / key items";
        d["Прочие предметы"] = "Other items";
        d["Дальность ESP"] = "ESP distance";
        d["Предметы - дальность"] = "Items distance";
        d["Контейнеры - дальность"] = "Containers distance";
        d["Враги - дальность"] = "Enemies distance";
        d["NPC - дальность"] = "NPC distance";
        d["NPC / союзники / торговцы - дальность"] = "NPC / ally / merchant distance";
        d["Отображение ESP"] = "ESP display";
        d["Название"] = "Name";
        d["Расстояние"] = "Distance";
        d["HP врагов / NPC"] = "Enemy / NPC HP";
        d["HP существ / NPC"] = "Creature / NPC HP";
        d["Полоски HP"] = "HP bars";
        d["Ширина полоски HP"] = "HP bar width";
        d["Высота полоски HP"] = "HP bar height";
        d["Статус контейнеров / трупов"] = "Container / corpse status";
        d["Скрывать пустые контейнеры / трупы"] = "Hide empty containers / corpses";
        d["Иконки ESP"] = "ESP icon badges";
        d["Только иконки (без названий)"] = "Icons only (hide names)";
        d["Размер иконок ESP"] = "ESP icon size";
        d["Темный фон подписи"] = "Dark label background";
        d["Размер текста ESP"] = "ESP text size";
        d["Максимум подписей на экране"] = "Maximum labels on screen";
        d["Интервал сканирования"] = "Scan interval";
        d["Статус ESP"] = "ESP status";
        d["Цвета: предметы - голубой, контейнеры - оранжевый, враги - красный, NPC - зеленый, трупы / пустые - серый."] = "Colors: items - cyan, containers - orange, enemies - red, NPCs - green, corpses / empty - gray.";
        d["Цвета: враги - красный, союзники - зеленый, нейтральные - желтый, торговцы - фиолетовый."] = "Colors: hostiles - red, friendlies - green, neutral - yellow, merchants - purple.";
        d["ПЕРЕСКАНИРОВАТЬ ESP"] = "RESCAN ESP";
        d["ESP готов"] = "ESP ready";
        d["ESP включен"] = "ESP enabled";
        d["ESP выключен"] = "ESP disabled";
        d["Основные функции"] = "Core functions";
        d["Урон и расход ресурсов"] = "Damage and resources";
        d["Передвижение и физика"] = "Movement and physics";
        d["Скорость игры и время"] = "Game speed and time";
        d["Погода"] = "Weather";
        d["Принудительная погода"] = "Forced weather";
        d["Выбор погоды"] = "Weather selection";
        d["Автоматически / По умолчанию"] = "Automatic / Default";
        d["Используются только штатные пресеты и переходы игры. Автоматический режим возвращает управление игре."] = "Only native game presets and transitions are used. Automatic mode returns control to the game.";
        d["Полет"] = "Flight";
        d["Валюта и вес"] = "Currency and weight";
        d["Количество предметов"] = "Item quantities";
        d["Подсказка"] = "Hint";
        d["Фильтры предметов"] = "Item filters";
        d["Поиск"] = "Search";
        d["Опыт"] = "Experience";
        d["Скорость и время"] = "Speed and time";
        d["Игрок"] = "Player";
        d["Характеристики"] = "Attributes";
        d["Мастерство"] = "Proficiencies";
        d["Профили"] = "Profiles";
        d["Система"] = "System";
        d["Диагностика"] = "Diagnostics";
        d["Ошибки патчей"] = "Patch errors";
        d["Локализация"] = "Localization";
        d["Режим бога / игнорирование ударов"] = "God mode / ignore hits";
        d["Бесконечное здоровье"] = "Infinite health";
        d["Бесконечная мана"] = "Infinite mana";
        d["Бесконечная выносливость"] = "Infinite stamina";
        d["Бесконечная сила короля"] = "Infinite King's Power";
        d["Бесконечный кислород"] = "Infinite oxygen";
        d["Предметы не уменьшаются"] = "Items do not decrease";
        d["Режим скрытности"] = "Stealth mode";
        d["Простой взлом замков"] = "Easy lockpicking";
        d["Сверхурон / убийство с одного удара"] = "One-hit kills";
        d["Множитель урона"] = "Damage multiplier";
        d["Множитель защиты"] = "Defense multiplier";
        d["Скорость расхода маны"] = "Mana consumption rate";
        d["Скорость расхода выносливости"] = "Stamina consumption rate";
        d["Полет / свободное перемещение"] = "Flight / free movement";
        d["Нулевой вес предметов"] = "Zero item weight";
        d["Нулевой вес экипировки"] = "Zero equipment weight";
        d["Игнорировать требования крафта"] = "Ignore crafting requirements";
        d["Показывать скрытые / служебные предметы"] = "Show hidden / internal items";
        d["Бесконечный опыт"] = "Infinite XP";
        d["Множитель опыта"] = "XP multiplier";
        d["Бесконечный опыт мастерства"] = "Infinite proficiency XP";
        d["Множитель опыта мастерства"] = "Proficiency XP multiplier";
        d["Скорость игры"] = "Game speed";
        d["Скорость движения"] = "Movement speed";
        d["Высота прыжка"] = "Jump height";
        d["Нет урона от падений"] = "No fall damage";
        d["Заморозить время суток"] = "Freeze time of day";
        d["Скорость течения времени"] = "Time passage speed";
        d["Ускорение полета (Shift)"] = "Flight boost (Shift)";
        d["Эфирная паутина"] = "Ethereal cobweb";
        d["Деньги"] = "Money";
        d["Количество зелий"] = "Potion quantity";
        d["Количество расходников"] = "Consumable quantity";
        d["Количество материалов"] = "Material quantity";
        d["Количество выбранного предмета"] = "Selected item quantity";
        d["Уровень выбранного предмета"] = "Selected item level";
        d["Уровень игрока"] = "Player level";
        d["Очки характеристик"] = "Attribute points";
        d["Очки навыков"] = "Skill points";
        d["Сила"] = "Strength";
        d["Выносливость"] = "Endurance";
        d["Ловкость"] = "Dexterity";
        d["Духовность"] = "Spirituality";
        d["Практичность"] = "Practicality";
        d["Восприятие"] = "Perception";
        d["Одноручное оружие"] = "One-handed";
        d["Двуручное оружие"] = "Two-handed";
        d["Без оружия"] = "Unarmed";
        d["Блокирование"] = "Blocking";
        d["Атлетика"] = "Athletics";
        d["Легкая броня"] = "Light armor";
        d["Средняя броня"] = "Medium armor";
        d["Тяжелая броня"] = "Heavy armor";
        d["Стрельба"] = "Archery";
        d["Уклонение"] = "Evasion";
        d["Ловкость / акробатика"] = "Agility / acrobatics";
        d["Скрытность"] = "Sneak";
        d["Воровство"] = "Theft";
        d["Магия"] = "Magic";
        d["Алхимия"] = "Alchemy";
        d["Кулинария"] = "Cooking";
        d["Ремесло"] = "Handcrafting";
        d["ПРИМЕНИТЬ"] = "APPLY";
        d["Все"] = "All";
        d["Оружие"] = "Weapons";
        d["Броня"] = "Armor";
        d["Щиты"] = "Shields";
        d["Расходники"] = "Consumables";
        d["Материалы"] = "Materials";
        d["Украшения"] = "Jewelry";
        d["Самоцветы"] = "Gems";
        d["Книги"] = "Books";
        d["Ключи"] = "Keys";
        d["Инструменты"] = "Tools";
        d["Важные"] = "Important";
        d["Прочее"] = "Other";
        d["Ближнее"] = "Melee";
        d["Одноручное"] = "One-handed";
        d["Двуручное"] = "Two-handed";
        d["Кинжалы"] = "Daggers";
        d["Мечи"] = "Swords";
        d["Топоры"] = "Axes";
        d["Дробящее"] = "Blunt";
        d["Древковое"] = "Polearms";
        d["Дальнее"] = "Ranged";
        d["Луки"] = "Bows";
        d["Стрелы"] = "Arrows";
        d["Метательное"] = "Throwable";
        d["Жезлы"] = "Rods";
        d["Кулаки"] = "Fists";
        d["Спектральное"] = "Spectral";
        d["Все луки"] = "All bows";
        d["Короткие"] = "Short";
        d["Средние"] = "Medium";
        d["Тяжелые"] = "Heavy";
        d["Легкая"] = "Light";
        d["Средняя"] = "Medium";
        d["Тяжелая"] = "Heavy";
        d["Зелья"] = "Potions";
        d["Еда"] = "Food";
        d["Блюда"] = "Dishes";
        d["Рыба"] = "Fish";
        d["Алкоголь"] = "Alcohol";
        d["Лечение"] = "Health";
        d["Мана"] = "Mana";
        d["Баффы"] = "Buffs";
        d["Готовка"] = "Cooking";
        d["Крафт"] = "Crafting";
        d["Компоненты"] = "Components";
        d["Оружие · Магия"] = "Weapon · Magic";
        d["Оружие · Двуручное"] = "Weapon · Two-handed";
        d["Оружие · Одноручное"] = "Weapon · One-handed";
        d["Оружие · Дальнее"] = "Weapon · Ranged";
        d["Оружие · Кинжал"] = "Weapon · Dagger";
        d["Оружие · Меч"] = "Weapon · Sword";
        d["Оружие · Топор"] = "Weapon · Axe";
        d["Оружие · Дробящее"] = "Weapon · Blunt";
        d["Оружие · Древковое"] = "Weapon · Polearm";
        d["Оружие · Короткий лук"] = "Weapon · Short bow";
        d["Оружие · Средний лук"] = "Weapon · Medium bow";
        d["Оружие · Тяжелый лук"] = "Weapon · Heavy bow";
        d["Оружие · Стрела"] = "Weapon · Arrow";
        d["Оружие · Метательное"] = "Weapon · Throwable";
        d["Оружие · Жезл"] = "Weapon · Rod";
        d["Оружие · Кулаки"] = "Weapon · Fists";
        d["Броня · Легкая"] = "Armor · Light";
        d["Броня · Средняя"] = "Armor · Medium";
        d["Броня · Тяжелая"] = "Armor · Heavy";
        d["Расходник · Зелье"] = "Consumable · Potion";
        d["Расходник · Блюдо"] = "Consumable · Dish";
        d["Расходник · Еда"] = "Consumable · Food";
        d["Расходник · Рыба"] = "Consumable · Fish";
        d["Расходник · Алкоголь"] = "Consumable · Alcohol";
        d["Материал · Алхимия"] = "Material · Alchemy";
        d["Материал · Готовка"] = "Material · Cooking";
        d["Материал · Крафт"] = "Material · Crafting";
        d["ГРУППА"] = "GROUP";
        d["ТИП"] = "TYPE";
        d["ПОДТИП"] = "SUBTYPE";
        d["Название"] = "Name";
        d["ОБНОВИТЬ БАЗУ"] = "REFRESH DATABASE";
        d["ЗАГРУЗИТЬ БАЗУ"] = "LOAD DATABASE";
        d["РЕЗУЛЬТАТЫ"] = "RESULTS";
        d["КАРТОЧКА ПРЕДМЕТА"] = "ITEM DETAILS";
        d["Кол-во"] = "Qty";
        d["Уровень"] = "Level";
        d["ОБНОВИТЬ СТАТЫ"] = "REFRESH STATS";
        d["ПОЛУЧИТЬ ПРЕДМЕТ"] = "GET ITEM";
        d["Нажмите 'ЗАГРУЗИТЬ БАЗУ'. После этого можно выбрать группу, тип и искать только внутри выбранной категории."] = "Click 'LOAD DATABASE'. Then choose a group/type and search only inside that category.";
        d["Выберите предмет слева. Для оружия урон будет рассчитан игровыми методами с учетом текущего персонажа и выбранного уровня предмета."] = "Select an item on the left. Weapon damage is calculated by the game using the current character and selected item level.";
        d["Выберите предмет"] = "Select an item";
        d["Категория: "] = "Category: ";
        d["Качество: "] = "Quality: ";
        d["Вес: "] = "Weight: ";
        d["Базовая цена: "] = "Base price: ";
        d["Цена покупки: "] = "Buy price: ";
        d["Мин. уровень: "] = "Min. level: ";
        d["Складывается: "] = "Stackable: ";
        d["Описание:"] = "Description:";
        d["Урон как в игровом tooltip:"] = "Damage as shown in game tooltip:";
        d["Расчетный урон для текущего персонажа (ур. предмета "] = "Calculated damage for current character (item level ";
        d["Урон: "] = "Damage: ";
        d["Расчетный урон: игровой расчет вернул 0-0; ориентируйтесь на строку tooltip выше."] = "Calculated damage: the game returned 0-0; use the tooltip damage above.";
        d["Среднее значение: "] = "Average value: ";
        d["Базовый урон предмета: "] = "Base item damage: ";
        d["Дополнительные статы предмета:"] = "Additional item stats:";
        d["Блок"] = "Block";
        d["Пробитие брони"] = "Armor penetration";
        d["Сила удара"] = "Force damage";
        d["Урон стойкости"] = "Poise damage";
        d["Требования персонажа:\n"] = "Character requirements:\n";
        d["НЕ ХВАТАЕТ"] = "NOT MET";
        d["Язык интерфейса"] = "Interface language";
        d["Язык сохраняется в профиле. При следующем запуске язык последнего сохраненного профиля будет выбран автоматически."] = "Language is stored in the profile. On the next launch, the language from the last saved profile is selected automatically.";
        d["Профиль"] = "Profile";
        d["СОХРАНИТЬ"] = "SAVE";
        d["ОБНОВИТЬ"] = "REFRESH";
        d["ЗАГРУЗИТЬ"] = "LOAD";
        d["УДАЛИТЬ"] = "DELETE";
        d["Профиль хранит все переключатели, множители, значения редакторов, настройки полета и локализацию. Последний сохраненный профиль загружается автоматически при запуске."] = "A profile stores all toggles, multipliers, editor values, flight settings and interface language. The last saved profile loads automatically at startup.";
        d["Сохраненных профилей пока нет."] = "No saved profiles yet.";
        d["ОТКЛЮЧИТЬ ВСЕ ФУНКЦИИ"] = "DISABLE ALL FUNCTIONS";
        d["Кнопка отключает все активные переключаемые читы и восстанавливает временно измененные параметры. Уже примененные деньги, уровень, характеристики и выданные предметы не откатываются."] = "Disables all active toggle cheats and restores temporary changes. Already applied money, levels, stats and spawned items are not reverted.";
        d["Установлено Harmony-патчей: "] = "Harmony patches installed: ";
        d["Все патчи установлены без ошибок."] = "All patches installed successfully.";
        d["Повторно синхронизировать значения с персонажем"] = "Resync editor values from character";
        d["ВКЛ"] = "ON";
        d["ВЫКЛ"] = "OFF";
        d["Готов"] = "Ready";
        d["Все переключаемые функции отключены"] = "All toggle functions disabled";
        d["Выбранный предмет: не определен. Наведите курсор на предмет в инвентаре."] = "Selected item: not detected. Hover an item in the inventory.";
        d["Выбранный предмет: найден"] = "Selected item: found";
        d["Поиск, предпросмотр характеристик и выдача новых предметов теперь находятся на отдельной вкладке ITEM SPAWNER."] = "Search, stat preview and spawning new items are available on the ITEM SPAWNER tab.";
        d["WASD - движение по направлению камеры, Space - вверх, Ctrl - вниз, Shift - ускорение. Полет отключает коллизии персонажа."] = "WASD - move relative to camera, Space - up, Ctrl - down, Shift - boost. Flight disables player collisions.";
        d["Полет включен"] = "Flight enabled";
        d["Полет выключен"] = "Flight disabled";
        d["Полет: игрок не найден"] = "Flight: player not found";
        d["Оконный режим трейнера"] = "Trainer window mode";
        d["Полноэкранный режим окна"] = "Fullscreen trainer window";
        d["найден"] = "found";
        d["НЕ НАЙДЕН"] = "NOT FOUND";
        d["не загружен"] = "not loaded";
        d["включен"] = "enabled";
        d["выключен"] = "disabled";
        d["База предметов еще не загружена"] = "Item database is not loaded yet";
        d["Выберите предмет из списка"] = "Select an item from the list";
        d["Сначала выберите предмет"] = "Select an item first";
        d["Игрок не найден"] = "Player not found";
        d["HeroItems не найден"] = "HeroItems not found";
        d["Не удалось создать предмет"] = "Failed to create item";
        d["Выбранный предмет не найден - наведите на предмет в инвентаре"] = "Selected item not found - hover an item in the inventory";
        d["У выбранного предмета нет уровня"] = "The selected item has no level";
        d["тип не найден"] = "type not found";
        d["метод не найден"] = "method not found";
        d["prefix не найден"] = "prefix not found";
        d["postfix не найден"] = "postfix not found";
        _englishText = d;
    }

    static string L(string text)
    {
        if (text == null || Language == 1) return text;
        EnsureLocalization();
        string e;
        if (_englishText.TryGetValue(text, out e)) return e;
        if (text.IndexOf("Профиль сохранен: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Профиль сохранен: ", "Profile saved: ");
        if (text.IndexOf("Профиль загружен: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Профиль загружен: ", "Profile loaded: ");
        if (text.IndexOf("Профиль удален: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Профиль удален: ", "Profile deleted: ");
        if (text.IndexOf("Профиль не найден: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Профиль не найден: ", "Profile not found: ");
        if (text.IndexOf("Не удалось сохранить профиль: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Не удалось сохранить профиль: ", "Failed to save profile: ");
        if (text.IndexOf("Не удалось загрузить профиль: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Не удалось загрузить профиль: ", "Failed to load profile: ");
        if (text.IndexOf("Не удалось удалить профиль: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Не удалось удалить профиль: ", "Failed to delete profile: ");
        if (text.IndexOf("Ошибка профилей: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Ошибка профилей: ", "Profile error: ");
        if (text.IndexOf("Загружено шаблонов: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Загружено шаблонов: ", "Templates loaded: ");
        if (text.IndexOf("Ошибка базы предметов: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Ошибка базы предметов: ", "Item database error: ");
        if (text.IndexOf("Выбран предмет: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Выбран предмет: ", "Selected item: ");
        if (text.IndexOf("Получено: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Получено: ", "Received: ");
        if (text.IndexOf("Ошибка выдачи предмета: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Ошибка выдачи предмета: ", "Item spawn error: ");
        if (text.IndexOf("Найдено: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("Найдено: ", "Found: ");
        if (text.IndexOf("  |  показаны первые 120 - уточните фильтр или поиск", System.StringComparison.Ordinal) >= 0) text = text.Replace("  |  показаны первые 120 - уточните фильтр или поиск", "  |  first 120 shown - refine filter or search");
        if (text.IndexOf("  |  Фильтр: ", System.StringComparison.Ordinal) >= 0) text = text.Replace("  |  Фильтр: ", "  |  Filter: ");
        if (text.IndexOf("Требования персонажа:\n", System.StringComparison.Ordinal) >= 0) text = text.Replace("Требования персонажа:\n", "Character requirements:\n");
        if (text.IndexOf(": требуется ", System.StringComparison.Ordinal) >= 0) text = text.Replace(": требуется ", ": requires ");
        if (text.IndexOf(" | у вас ", System.StringComparison.Ordinal) >= 0) text = text.Replace(" | у вас ", " | you have ");
        if (text.IndexOf(" (ур. ", System.StringComparison.Ordinal) >= 0) text = text.Replace(" (ур. ", " (lvl ");
        if (text.IndexOf("Эфирная паутина: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Эфирная паутина: ", "Ethereal cobweb: ");
        if (text.IndexOf("Деньги: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Деньги: ", "Money: ");
        if (text.IndexOf("Зелья: изменено стопок ", System.StringComparison.Ordinal) == 0) text = text.Replace("Зелья: изменено стопок ", "Potions: stacks changed ");
        if (text.IndexOf("Расходники: изменено стопок ", System.StringComparison.Ordinal) == 0) text = text.Replace("Расходники: изменено стопок ", "Consumables: stacks changed ");
        if (text.IndexOf("Материалы: изменено стопок ", System.StringComparison.Ordinal) == 0) text = text.Replace("Материалы: изменено стопок ", "Materials: stacks changed ");
        if (text.IndexOf("Количество выбранного предмета: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Количество выбранного предмета: ", "Selected item quantity: ");
        if (text.IndexOf("Уровень выбранного предмета: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Уровень выбранного предмета: ", "Selected item level: ");
        if (text.IndexOf("Уровень игрока: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Уровень игрока: ", "Player level: ");
        if (text.IndexOf("Очки характеристик: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Очки характеристик: ", "Attribute points: ");
        if (text.IndexOf("Очки навыков: ", System.StringComparison.Ordinal) == 0) text = text.Replace("Очки навыков: ", "Skill points: ");
        string[] statNamesRu = new string[] { "Сила: ", "Выносливость: ", "Ловкость: ", "Духовность: ", "Практичность: ", "Восприятие: ", "Одноручное: ", "Двуручное: ", "Без оружия: ", "Блокирование: ", "Атлетика: ", "Легкая броня: ", "Средняя броня: ", "Тяжелая броня: ", "Стрельба: ", "Уклонение: ", "Акробатика: ", "Скрытность: ", "Воровство: ", "Магия: ", "Алхимия: ", "Кулинария: ", "Ремесло: " };
        string[] statNamesEn = new string[] { "Strength: ", "Endurance: ", "Dexterity: ", "Spirituality: ", "Practicality: ", "Perception: ", "One-handed: ", "Two-handed: ", "Unarmed: ", "Blocking: ", "Athletics: ", "Light armor: ", "Medium armor: ", "Heavy armor: ", "Archery: ", "Evasion: ", "Acrobatics: ", "Sneak: ", "Theft: ", "Magic: ", "Alchemy: ", "Cooking: ", "Handcrafting: " };
        for (int i = 0; i < statNamesRu.Length; i++)
        {
            if (text.IndexOf(statNamesRu[i], System.StringComparison.Ordinal) == 0)
            {
                text = statNamesEn[i] + text.Substring(statNamesRu[i].Length);
                break;
            }
        }
        return text;
    }

    static string LastProfileMarkerPath()
    {
        return System.IO.Path.Combine(BepInEx.Paths.ConfigPath, "FoATrainer_LastProfile.txt");
    }

    static void RememberLastSavedProfile(string name)
    {
        try
        {
            System.IO.File.WriteAllText(LastProfileMarkerPath(), SafeProfileName(name), System.Text.Encoding.UTF8);
        }
        catch { }
    }

    static string FindLastSavedProfile()
    {
        try
        {
            string markerPath = LastProfileMarkerPath();
            if (System.IO.File.Exists(markerPath))
            {
                string marked = System.IO.File.ReadAllText(markerPath, System.Text.Encoding.UTF8).Trim();
                if (marked.Length > 0 && System.IO.File.Exists(ProfilePath(marked))) return marked;
            }

            if (!_profilesScanned) RefreshProfiles();
            string latest = null;
            System.DateTime latestTime = System.DateTime.MinValue;
            for (int i = 0; i < _profiles.Count; i++)
            {
                string name = _profiles[i];
                string path = ProfilePath(name);
                if (!System.IO.File.Exists(path)) continue;
                System.DateTime t = System.IO.File.GetLastWriteTimeUtc(path);
                if (latest == null || t > latestTime)
                {
                    latest = name;
                    latestTime = t;
                }
            }
            return latest;
        }
        catch { return null; }
    }

    static void LoadLanguageOnlyFromProfile(string name)
    {
        try
        {
            string path = ProfilePath(name);
            if (!System.IO.File.Exists(path)) return;
            string[] lines = System.IO.File.ReadAllLines(path, System.Text.Encoding.UTF8);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].StartsWith("Language=")) continue;
                string raw = lines[i].Substring("Language=".Length);
                int lang = System.Convert.ToInt32(raw, System.Globalization.CultureInfo.InvariantCulture);
                Language = lang == 1 ? 1 : 0;
                return;
            }
        }
        catch { }
    }

    static void PrepareStartupProfile()
    {
        _startupProfileName = FindLastSavedProfile();
        if (_startupProfileName == null || _startupProfileName.Length == 0) return;
        _profileName = _startupProfileName;
        LoadLanguageOnlyFromProfile(_startupProfileName);
    }

    static void PrimeStartupProfile()
    {
        if (_startupProfileName == null || _startupProfileName.Length == 0) return;
        if (!System.IO.File.Exists(ProfilePath(_startupProfileName))) return;
        // Load saved switches immediately. Most toggles do not need a live Hero,
        // so their state is already correct while the save is still loading.
        LoadProfile(_startupProfileName);
        _autoProfileLoadAttempted = false;
        _startupProfileLoadAttempts = 0;
        _startupProfileLoadAfter = UnityEngine.Time.realtimeSinceStartup + 1.25f;
    }

    static bool HeroReadyForStartupProfile(object hero)
    {
        if (hero == null) return false;
        // Wait until the core character stat containers exist. This prevents the
        // first automatic load from being consumed by a partially initialized Hero.
        if (HeroStats(hero) == null) return false;
        if (CharacterStats(hero) == null) return false;
        if (RPGStats(hero) == null) return false;
        return true;
    }

    static void TryAutoLoadStartupProfile(object hero)
    {
        if (_autoProfileLoadAttempted) return;
        if (_startupProfileName == null || _startupProfileName.Length == 0)
        {
            _autoProfileLoadAttempted = true;
            return;
        }
        if (UnityEngine.Time.realtimeSinceStartup < _startupProfileLoadAfter) return;
        if (!HeroReadyForStartupProfile(hero)) return;

        bool ok = AutoLoadStartupProfile();
        if (ok)
        {
            _autoProfileLoadAttempted = true;
            _log.LogInfo("[FoATrainer] Startup profile applied: " + _startupProfileName);
            return;
        }

        _startupProfileLoadAttempts++;
        if (_startupProfileLoadAttempts >= 5)
        {
            _autoProfileLoadAttempted = true;
            _log.LogWarning("[FoATrainer] Startup profile could not be applied after retries: " + _startupProfileName);
        }
        else
        {
            _startupProfileLoadAfter = UnityEngine.Time.realtimeSinceStartup + 1.0f;
        }
    }

    static bool AutoLoadStartupProfile()
    {
        if (_startupProfileName == null || _startupProfileName.Length == 0) return false;
        if (!System.IO.File.Exists(ProfilePath(_startupProfileName))) return false;
        return LoadProfile(_startupProfileName);
    }

    // ============================ V8: Profiles ============================
    static string ProfilesDirectory()
    {
        return System.IO.Path.Combine(BepInEx.Paths.ConfigPath, "FoATrainer_Profiles");
    }

    static string SafeProfileName(string name)
    {
        string n = name == null ? "" : name.Trim();
        if (n.Length == 0) n = Language == 1 ? "Профиль" : "Profile";
        char[] bad = System.IO.Path.GetInvalidFileNameChars();
        for (int i = 0; i < bad.Length; i++) n = n.Replace(bad[i], '_');
        return n;
    }

    static string ProfilePath(string name)
    {
        return System.IO.Path.Combine(ProfilesDirectory(), SafeProfileName(name) + ".profile");
    }

    static bool IsSpecialProfileToggle(string name)
    {
        return name == "StealthMode" || name == "EasyLockPicking" || name == "ManaRateEnabled" || name == "StaminaRateEnabled" ||
               name == "MovementSpeedEnabled" || name == "JumpHeightEnabled" || name == "GameSpeedEnabled" || name == "FlightEnabled";
    }

    static void RefreshProfiles()
    {
        try
        {
            string dir = ProfilesDirectory();
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
            _profiles.Clear();
            string[] files = System.IO.Directory.GetFiles(dir, "*.profile");
            for (int i = 0; i < files.Length; i++)
            {
                string profileFile = files[i];
                int slash = profileFile.LastIndexOf('\\');
                if (slash >= 0 && slash + 1 < profileFile.Length) profileFile = profileFile.Substring(slash + 1);
                if (profileFile.EndsWith(".profile")) profileFile = profileFile.Substring(0, profileFile.Length - 8);
                if (profileFile.Length > 0) _profiles.Add(profileFile);
            }
            _profiles.Sort();
            _profilesScanned = true;
        }
        catch (System.Exception ex)
        {
            _lastAction = "Ошибка профилей: " + ex.Message;
        }
    }

    static void SaveProfile(string name)
    {
        try
        {
            if (!_profilesScanned) RefreshProfiles();
            string safe = SafeProfileName(name);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("FoATrainerProfile=1");
            System.Reflection.FieldInfo[] fields = typeof(FoATrainerRuntime).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            for (int i = 0; i < fields.Length; i++)
            {
                System.Reflection.FieldInfo f = fields[i];
                if (f.FieldType == typeof(bool) || f.FieldType == typeof(int) || f.FieldType == typeof(float))
                {
                    object v = f.GetValue(null);
                    string text;
                    if (f.FieldType == typeof(float)) text = System.Convert.ToSingle(v).ToString("R", System.Globalization.CultureInfo.InvariantCulture);
                    else text = System.Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture);
                    sb.AppendLine(f.Name + "=" + text);
                }
            }
            System.IO.File.WriteAllText(ProfilePath(safe), sb.ToString(), System.Text.Encoding.UTF8);
            RememberLastSavedProfile(safe);
            _startupProfileName = safe;
            _profileName = safe;
            _autoProfileLoadAttempted = true;
            RefreshProfiles();
            _lastAction = "Профиль сохранен: " + safe;
        }
        catch (System.Exception ex) { _lastAction = "Не удалось сохранить профиль: " + ex.Message; }
    }

    static bool LoadProfile(string name)
    {
        try
        {
            string path = ProfilePath(name);
            if (!System.IO.File.Exists(path)) { _lastAction = "Профиль не найден: " + name; return false; }
            string[] lines = System.IO.File.ReadAllLines(path, System.Text.Encoding.UTF8);
            System.Collections.Generic.Dictionary<string, string> values = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < lines.Length; i++)
            {
                int eq = lines[i].IndexOf('=');
                if (eq <= 0) continue;
                values[lines[i].Substring(0, eq)] = lines[i].Substring(eq + 1);
            }

            // Weather is stored independently in BepInEx Config and is not part of profiles.
            DisableAllFunctions(false);
            // Backward-compatible defaults for profiles created before the new ESP categories.
            // Friendly NPCs previously followed the generic NPC toggle; merchants were usually
            // included in enemies because EnemyBaseClass was the old classifier.
            string legacyValue;
            if (!values.ContainsKey("EspFriendlies"))
            {
                if (values.TryGetValue("EspNpcs", out legacyValue))
                {
                    try { EspFriendlies = System.Convert.ToBoolean(legacyValue, System.Globalization.CultureInfo.InvariantCulture); } catch { EspFriendlies = true; }
                }
                else EspFriendlies = true;
            }
            if (!values.ContainsKey("EspMerchants"))
            {
                if (values.TryGetValue("EspEnemies", out legacyValue))
                {
                    try { EspMerchants = System.Convert.ToBoolean(legacyValue, System.Globalization.CultureInfo.InvariantCulture); } catch { EspMerchants = true; }
                }
                else EspMerchants = true;
            }
            if (!values.ContainsKey("EspHealthBarHeight")) EspHealthBarHeight = 3;

            System.Reflection.FieldInfo[] fields = typeof(FoATrainerRuntime).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            for (int i = 0; i < fields.Length; i++)
            {
                System.Reflection.FieldInfo f = fields[i];
                string text;
                if (!values.TryGetValue(f.Name, out text)) continue;
                if (f.FieldType == typeof(bool) && !IsSpecialProfileToggle(f.Name))
                {
                    try { f.SetValue(null, System.Convert.ToBoolean(text, System.Globalization.CultureInfo.InvariantCulture)); } catch { }
                }
                else if (f.FieldType == typeof(int))
                {
                    try { f.SetValue(null, System.Convert.ToInt32(text, System.Globalization.CultureInfo.InvariantCulture)); } catch { }
                }
                else if (f.FieldType == typeof(float))
                {
                    try { f.SetValue(null, System.Convert.ToSingle(text, System.Globalization.CultureInfo.InvariantCulture)); } catch { }
                }
            }

            string b;
            if (values.TryGetValue("StealthMode", out b)) SetStealth(System.Convert.ToBoolean(b));
            if (values.TryGetValue("EasyLockPicking", out b)) SetEasyLock(System.Convert.ToBoolean(b));
            if (values.TryGetValue("ManaRateEnabled", out b)) SetManaRate(System.Convert.ToBoolean(b));
            if (values.TryGetValue("StaminaRateEnabled", out b)) SetStaminaRate(System.Convert.ToBoolean(b));
            if (values.TryGetValue("MovementSpeedEnabled", out b)) SetMovementSpeed(System.Convert.ToBoolean(b));
            if (values.TryGetValue("JumpHeightEnabled", out b)) SetJumpHeight(System.Convert.ToBoolean(b));
            if (values.TryGetValue("GameSpeedEnabled", out b)) SetGameSpeed(System.Convert.ToBoolean(b));
            if (values.TryGetValue("FlightEnabled", out b)) SetFlight(System.Convert.ToBoolean(b));

            _profileName = name;
            _lastAction = "Профиль загружен: " + name;
            return true;
        }
        catch (System.Exception ex)
        {
            _lastAction = "Не удалось загрузить профиль: " + ex.Message;
            _log.LogWarning("[FoATrainer] Profile load error: " + ex.Message);
            return false;
        }
    }

    static void DeleteProfile(string name)
    {
        try
        {
            string path = ProfilePath(name);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            RefreshProfiles();
            _lastAction = "Профиль удален: " + name;
        }
        catch (System.Exception ex) { _lastAction = "Не удалось удалить профиль: " + ex.Message; }
    }

    // ============================ V8: Item Spawner ============================
    static string ObjString(object value, string fallback)
    {
        if (value == null) return fallback;
        try { string s = value as string; if (s != null && s.Length > 0) return s; return value.ToString(); }
        catch { return fallback; }
    }

    static string TemplateName(object template)
    {
        if (template == null) return "<null>";
        string n = ObjString(GetProp(template, "ItemName"), "");
        if (n.Length == 0) n = ObjString(GetProp(template, "name"), "");
        if (n.Length == 0) n = ObjString(template, "Без названия");
        return n;
    }

    static string TemplateSearchBlob(object template)
    {
        if (template == null) return "";
        return TemplateName(template) + " " + ObjString(GetProp(template, "name"), "") + " " + ObjString(GetProp(template, "GUID"), "") + " " + ObjString(template, "");
    }

    static int TemplateGroupIndex(object t)
    {
        if (t == null) return 12;
        if (ToBool(GetProp(t, "IsShield"))) return 3;
        if (ToBool(GetProp(t, "IsArmor"))) return 2;
        if (ToBool(GetProp(t, "IsWeapon"))) return 1;
        if (ToBool(GetProp(t, "IsPotion")) || ToBool(GetProp(t, "IsConsumable")) || ToBool(GetProp(t, "IsPlainFood")) || ToBool(GetProp(t, "IsDish")) || ToBool(GetProp(t, "IsFish")) || ToBool(GetProp(t, "IsAlcohol"))) return 4;
        if (ToBool(GetProp(t, "IsCrafting")) || ToBool(GetProp(t, "IsComponent"))) return 5;
        if (ToBool(GetProp(t, "IsJewelry"))) return 6;
        if (ToBool(GetProp(t, "IsGem"))) return 7;
        if (ToBool(GetProp(t, "IsReadable"))) return 8;
        if (ToBool(GetProp(t, "IsKey"))) return 9;
        if (ToBool(GetProp(t, "IsTool"))) return 10;
        if (ToBool(GetProp(t, "IsImportantItem"))) return 11;
        return 12;
    }

    static string TemplateKind(object t)
    {
        int g = TemplateGroupIndex(t);
        if (g >= 0 && g < _itemGroupNames.Length) return _itemGroupNames[g];
        return "Предмет";
    }

    static string TemplateDetailedKind(object t)
    {
        int g = TemplateGroupIndex(t);
        if (g == 1)
        {
            if (ToBool(GetProp(t, "IsShortBow"))) return "Оружие · Короткий лук";
            if (ToBool(GetProp(t, "IsMediumBow"))) return "Оружие · Средний лук";
            if (ToBool(GetProp(t, "IsHeavyBow"))) return "Оружие · Тяжелый лук";
            if (ToBool(GetProp(t, "IsDagger"))) return "Оружие · Кинжал";
            if (ToBool(GetProp(t, "IsSword"))) return "Оружие · Меч";
            if (ToBool(GetProp(t, "IsAxe"))) return "Оружие · Топор";
            if (ToBool(GetProp(t, "IsBlunt"))) return "Оружие · Дробящее";
            if (ToBool(GetProp(t, "IsPolearm"))) return "Оружие · Древковое";
            if (ToBool(GetProp(t, "IsArrow"))) return "Оружие · Стрела";
            if (ToBool(GetProp(t, "IsThrowable"))) return "Оружие · Метательное";
            if (ToBool(GetProp(t, "IsRod"))) return "Оружие · Жезл";
            if (ToBool(GetProp(t, "IsFists"))) return "Оружие · Кулаки";
            if (ToBool(GetProp(t, "IsMagic"))) return "Оружие · Магия";
            if (ToBool(GetProp(t, "IsTwoHanded"))) return "Оружие · Двуручное";
            if (ToBool(GetProp(t, "IsOneHanded"))) return "Оружие · Одноручное";
            if (ToBool(GetProp(t, "IsRanged"))) return "Оружие · Дальнее";
            return "Оружие";
        }
        if (g == 2)
        {
            if (ToBool(GetProp(t, "IsLightArmor"))) return "Броня · Легкая";
            if (ToBool(GetProp(t, "IsMediumArmor"))) return "Броня · Средняя";
            if (ToBool(GetProp(t, "IsHeavyArmor"))) return "Броня · Тяжелая";
        }
        if (g == 4)
        {
            if (ToBool(GetProp(t, "IsPotion"))) return "Расходник · Зелье";
            if (ToBool(GetProp(t, "IsDish"))) return "Расходник · Блюдо";
            if (ToBool(GetProp(t, "IsPlainFood"))) return "Расходник · Еда";
            if (ToBool(GetProp(t, "IsFish"))) return "Расходник · Рыба";
            if (ToBool(GetProp(t, "IsAlcohol"))) return "Расходник · Алкоголь";
        }
        if (g == 5)
        {
            if (ToBool(GetProp(t, "IsAlchemyComponent"))) return "Материал · Алхимия";
            if (ToBool(GetProp(t, "IsCookingComponent"))) return "Материал · Готовка";
            if (ToBool(GetProp(t, "IsCraftingComponent"))) return "Материал · Крафт";
        }
        return TemplateKind(t);
    }

    static string[] ItemSubtypeNames(int group)
    {
        if (group == 1) return _weaponSubtypeNames;
        if (group == 2) return _armorSubtypeNames;
        if (group == 4) return _consumableSubtypeNames;
        if (group == 5) return _materialSubtypeNames;
        return _singleSubtypeNames;
    }

    static bool MatchesItemSubtype(object t, int group, int subtype)
    {
        if (subtype <= 0) return true;
        if (group == 1)
        {
            if (subtype == 1) return ToBool(GetProp(t, "IsMelee"));
            if (subtype == 2) return ToBool(GetProp(t, "IsOneHanded"));
            if (subtype == 3) return ToBool(GetProp(t, "IsTwoHanded"));
            if (subtype == 4) return ToBool(GetProp(t, "IsDagger"));
            if (subtype == 5) return ToBool(GetProp(t, "IsSword"));
            if (subtype == 6) return ToBool(GetProp(t, "IsAxe"));
            if (subtype == 7) return ToBool(GetProp(t, "IsBlunt"));
            if (subtype == 8) return ToBool(GetProp(t, "IsPolearm"));
            if (subtype == 9) return ToBool(GetProp(t, "IsRanged"));
            if (subtype == 10) return ToBool(GetProp(t, "IsShortBow")) || ToBool(GetProp(t, "IsMediumBow")) || ToBool(GetProp(t, "IsHeavyBow"));
            if (subtype == 11) return ToBool(GetProp(t, "IsArrow"));
            if (subtype == 12) return ToBool(GetProp(t, "IsThrowable"));
            if (subtype == 13) return ToBool(GetProp(t, "IsMagic"));
            if (subtype == 14) return ToBool(GetProp(t, "IsRod"));
            if (subtype == 15) return ToBool(GetProp(t, "IsFists"));
            if (subtype == 16) return ToBool(GetProp(t, "IsSpectralWeapon"));
            if (subtype == 17) return ToBool(GetProp(t, "IsChaingun"));
            if (subtype == 18) return ToBool(GetProp(t, "IsSoulCube"));
            return true;
        }
        if (group == 2)
        {
            if (subtype == 1) return ToBool(GetProp(t, "IsLightArmor"));
            if (subtype == 2) return ToBool(GetProp(t, "IsMediumArmor"));
            if (subtype == 3) return ToBool(GetProp(t, "IsHeavyArmor"));
            return true;
        }
        if (group == 4)
        {
            if (subtype == 1) return ToBool(GetProp(t, "IsPotion"));
            if (subtype == 2) return ToBool(GetProp(t, "IsPlainFood"));
            if (subtype == 3) return ToBool(GetProp(t, "IsDish"));
            if (subtype == 4) return ToBool(GetProp(t, "IsFish"));
            if (subtype == 5) return ToBool(GetProp(t, "IsAlcohol"));
            if (subtype == 6) return ToBool(GetProp(t, "ConsumableModifiesHealth"));
            if (subtype == 7) return ToBool(GetProp(t, "ConsumableModifiesMana"));
            if (subtype == 8) return ToBool(GetProp(t, "ConsumableStamina"));
            if (subtype == 9) return ToBool(GetProp(t, "IsBuffApplier")) || ToBool(GetProp(t, "ConsumablePotionOther"));
            return true;
        }
        if (group == 5)
        {
            if (subtype == 1) return ToBool(GetProp(t, "IsAlchemyComponent"));
            if (subtype == 2) return ToBool(GetProp(t, "IsCookingComponent"));
            if (subtype == 3) return ToBool(GetProp(t, "IsCraftingComponent"));
            if (subtype == 4) return ToBool(GetProp(t, "IsComponent"));
            return true;
        }
        return true;
    }

    static string[] ItemDetailNames(int group, int subtype)
    {
        if (group == 1 && subtype == 10) return _bowDetailNames;
        return _singleSubtypeNames;
    }

    static bool MatchesItemDetail(object t, int group, int subtype, int detail)
    {
        if (detail <= 0) return true;
        if (group == 1 && subtype == 10)
        {
            if (detail == 1) return ToBool(GetProp(t, "IsShortBow"));
            if (detail == 2) return ToBool(GetProp(t, "IsMediumBow"));
            if (detail == 3) return ToBool(GetProp(t, "IsHeavyBow"));
        }
        return true;
    }

    static bool MatchesItemFilter(object t)
    {
        if (t == null) return false;
        if (_itemGroup > 0 && TemplateGroupIndex(t) != _itemGroup) return false;
        if (_itemGroup > 0 && !MatchesItemSubtype(t, _itemGroup, _itemSubtype)) return false;
        if (_itemGroup > 0 && !MatchesItemDetail(t, _itemGroup, _itemSubtype, _itemDetail)) return false;
        return true;
    }

    static void LoadItemTemplates()
    {
        _itemTemplates.Clear();
        try
        {
            System.Type utils = FindType("Awaken.TG.Main.Heroes.Items.ItemUtils");
            System.Reflection.MethodInfo mi = utils == null ? null : FindMethod(utils, "ItemTemplates", 1);
            if (mi == null) throw new System.Exception("ItemUtils.ItemTemplates не найден");
            object result = null;
            try { result = mi.Invoke(null, new object[] { null }); }
            catch
            {
                System.Collections.Generic.List<string> tags = new System.Collections.Generic.List<string>();
                result = mi.Invoke(null, new object[] { tags });
            }
            System.Collections.IEnumerable e = result as System.Collections.IEnumerable;
            if (e == null) throw new System.Exception("ItemTemplates вернул пустой результат");
            foreach (object t in e) if (t != null) _itemTemplates.Add(t);
            _itemTemplatesLoaded = true;
            _itemTemplatesStatus = "Загружено шаблонов: " + _itemTemplates.Count;
            _lastAction = _itemTemplatesStatus;
        }
        catch (System.Exception ex)
        {
            _itemTemplatesLoaded = false;
            _itemTemplatesStatus = "Ошибка базы предметов: " + ex.Message;
            _lastAction = _itemTemplatesStatus;
        }
    }

    static object CreateItemForTemplate(object template, int quantity, int level)
    {
        if (template == null) return null;
        System.Type itemType = FindType("Awaken.TG.Main.Heroes.Items.Item");
        if (itemType == null) return null;
        quantity = System.Math.Max(1, quantity);
        level = System.Math.Max(1, level);
        try
        {
            System.Reflection.ConstructorInfo[] ctors = itemType.GetConstructors(AllFlags);
            for (int i = 0; i < ctors.Length; i++)
            {
                System.Reflection.ParameterInfo[] ps = ctors[i].GetParameters();
                if (ps.Length == 6)
                {
                    return ctors[i].Invoke(new object[] { template, quantity, level, level, 0, false });
                }
            }
            for (int i = 0; i < ctors.Length; i++)
            {
                System.Reflection.ParameterInfo[] ps = ctors[i].GetParameters();
                if (ps.Length == 2) return ctors[i].Invoke(new object[] { template, quantity });
            }
        }
        catch (System.Exception ex) { _log.LogWarning("[FoATrainer] Create item: " + ex.Message); }
        return null;
    }

    static bool InitializePreviewItem(object item)
    {
        if (item == null) return false;
        try
        {
            System.Type worldType = FindType("Awaken.TG.MVC.World");
            if (worldType == null) return false;
            System.Reflection.MethodInfo[] methods = worldType.GetMethods(AllFlags);
            for (int i = 0; i < methods.Length; i++)
            {
                System.Reflection.MethodInfo m = methods[i];
                if (m.Name != "Add" || !m.IsGenericMethodDefinition) continue;
                if (m.GetParameters().Length != 1) continue;
                System.Reflection.MethodInfo closed = m.MakeGenericMethod(new System.Type[] { item.GetType() });
                closed.Invoke(null, new object[] { item });
                return ToBool(GetProp(item, "IsInitialized"));
            }
        }
        catch (System.Exception ex)
        {
            if (_log != null) _log.LogWarning("[FoATrainer] Preview World.Add: " + ex.Message);
        }
        return ToBool(GetProp(item, "IsInitialized"));
    }

    static void DiscardPreviewItem(object item)
    {
        if (item == null) return;
        try
        {
            if (!ToBool(GetProp(item, "IsInitialized"))) return;
            System.Reflection.MethodInfo discard = FindMethod(item.GetType(), "Discard", 0);
            if (discard != null) discard.Invoke(item, null);
        }
        catch { }
    }

    static string BuildItemRequirements(object template, object preview)
    {
        if (template == null) return "";
        float strength = 0f;
        float dexterity = 0f;
        float spirituality = 0f;
        float perception = 0f;
        float endurance = 0f;
        float practicality = 0f;

        try
        {
            object req = preview == null ? null : GetProp(preview, "StatsRequirements");
            if (req != null)
            {
                strength = StatModified(GetProp(req, "StrengthRequired"));
                dexterity = StatModified(GetProp(req, "DexterityRequired"));
                spirituality = StatModified(GetProp(req, "SpiritualityRequired"));
                perception = StatModified(GetProp(req, "PerceptionRequired"));
                endurance = StatModified(GetProp(req, "EnduranceRequired"));
                practicality = StatModified(GetProp(req, "PracticalityRequired"));
            }
        }
        catch { }

        if (strength <= 0f && dexterity <= 0f && spirituality <= 0f && perception <= 0f && endurance <= 0f && practicality <= 0f)
        {
            try
            {
                System.Type attachmentType = FindType("Awaken.TG.Main.Heroes.Items.Attachments.ItemStatsRequirementsAttachment");
                UnityEngine.Component component = template as UnityEngine.Component;
                if (attachmentType != null && component != null)
                {
                    object attachment = component.GetComponent(attachmentType);
                    if (attachment != null)
                    {
                        strength = ToFloat(GetProp(attachment, "strengthRequired"), 0f);
                        dexterity = ToFloat(GetProp(attachment, "dexterityRequired"), 0f);
                        spirituality = ToFloat(GetProp(attachment, "spiritualityRequired"), 0f);
                        perception = ToFloat(GetProp(attachment, "perceptionRequired"), 0f);
                        endurance = ToFloat(GetProp(attachment, "enduranceRequired"), 0f);
                        practicality = ToFloat(GetProp(attachment, "practicalityRequired"), 0f);
                    }
                }
            }
            catch { }
        }

        object hero = Hero();
        object rpg = hero == null ? null : RPGStats(hero);
        float heroStrength = rpg == null ? 0f : StatModified(GetProp(rpg, "Strength"));
        float heroDexterity = rpg == null ? 0f : StatModified(GetProp(rpg, "Dexterity"));
        float heroSpirituality = rpg == null ? 0f : StatModified(GetProp(rpg, "Spirituality"));
        float heroPerception = rpg == null ? 0f : StatModified(GetProp(rpg, "Perception"));
        float heroEndurance = rpg == null ? 0f : StatModified(GetProp(rpg, "Endurance"));
        float heroPracticality = rpg == null ? 0f : StatModified(GetProp(rpg, "Practicality"));

        string result = "";
        result = AddRequirementText(result, "Сила", strength, heroStrength);
        result = AddRequirementText(result, "Ловкость", dexterity, heroDexterity);
        result = AddRequirementText(result, "Духовность", spirituality, heroSpirituality);
        result = AddRequirementText(result, "Восприятие", perception, heroPerception);
        result = AddRequirementText(result, "Выносливость", endurance, heroEndurance);
        result = AddRequirementText(result, "Практичность", practicality, heroPracticality);
        if (result.Length == 0) return "";
        return L("Требования персонажа:\n") + result;
    }

    static string AddRequirementText(string current, string label, float requiredValue, float heroValue)
    {
        if (requiredValue <= 0f) return current;
        int required = (int)System.Math.Ceiling(requiredValue);
        int actual = (int)System.Math.Floor(heroValue + 0.0001f);
        bool met = heroValue + 0.0001f >= requiredValue;
        string color = met ? "#63D17A" : "#FF6B61";
        string state = met ? "OK" : L("НЕ ХВАТАЕТ");
        if (current.Length > 0) current = current + "\n";
        return current + "<color=" + color + ">" + L(label) + (Language == 1 ? ": требуется " : ": requires ") + required.ToString(System.Globalization.CultureInfo.InvariantCulture) +
               (Language == 1 ? " | у вас " : " | you have ") + actual.ToString(System.Globalization.CultureInfo.InvariantCulture) + " | " + state + "</color>";
    }

    static string StatLine(string name, object stat)
    {
        if (stat == null) return "";
        float v = StatModified(stat);
        return name + ": " + v.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "\n";
    }

    static string BuildSpawnPreview(object template)
    {
        if (template == null) return L("Выберите предмет");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine(TemplateName(template));
        sb.AppendLine(L("Категория: ") + L(TemplateDetailedKind(template)));
        sb.AppendLine(L("Качество: ") + ObjString(GetProp(template, "Quality"), "-") + "   Tier: " + ObjString(GetProp(template, "Tier"), "-"));
        sb.AppendLine(L("Вес: ") + ToFloat(GetProp(template, "Weight"), 0f).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture) +
                      "   " + L("Базовая цена: ") + ToInt(GetProp(template, "BasePrice"), 0) +
                      "   " + L("Цена покупки: ") + ToFloat(GetProp(template, "BuyPrice"), 0f).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
        sb.AppendLine(L("Мин. уровень: ") + ToInt(GetProp(template, "MinimumItemLevel"), 0) + "   " + L("Складывается: ") + (ToBool(GetProp(template, "CanStack")) ? (Language == 1 ? "да" : "yes") : (Language == 1 ? "нет" : "no")));

        object hero = Hero();
        object preview = null;
        bool previewInitialized = false;
        try
        {
            preview = CreateItemForTemplate(template, 1, _spawnLevel);
            previewInitialized = InitializePreviewItem(preview);
        }
        catch
        {
            preview = null;
            previewInitialized = false;
        }

        string requirements = BuildItemRequirements(template, preview);
        if (requirements.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine(requirements);
        }

        try
        {
            System.Type utils = FindType("Awaken.TG.Main.Heroes.Items.ItemUtils");
            if (utils != null && hero != null)
            {
                System.Reflection.MethodInfo desc = FindMethodByParameterType(utils, "GetTemplateDescription", 2, 0, "Awaken.TG.Main.Heroes.Items.ItemTemplate");
                if (desc != null)
                {
                    string d = ObjString(desc.Invoke(null, new object[] { template, hero }), "");
                    if (d.Length > 0) { sb.AppendLine(); sb.AppendLine(L("Описание:")); sb.AppendLine(d); }
                }

                if (ToBool(GetProp(template, "IsWeapon")))
                {
                    string ws = "";
                    if (preview != null && previewInitialized)
                    {
                        System.Reflection.MethodInfo wstatsItem = FindMethodByParameterType(utils, "DisplayWeaponStats", 2, 1, "Awaken.TG.Main.Heroes.Items.Item");
                        if (wstatsItem != null) ws = ObjString(wstatsItem.Invoke(null, new object[] { hero, preview }), "");
                    }
                    if (ws.Length == 0)
                    {
                        System.Reflection.MethodInfo wstatsTemplate = FindMethodByParameterType(utils, "DisplayWeaponStats", 2, 1, "Awaken.TG.Main.Heroes.Items.ItemTemplate");
                        if (wstatsTemplate != null) ws = ObjString(wstatsTemplate.Invoke(null, new object[] { hero, template }), "");
                    }
                    if (ws.Length > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine(L("Урон как в игровом tooltip:"));
                        sb.AppendLine(ws);
                    }
                }
            }
        }
        catch { }

        if (preview != null)
        {
            try
            {
                if (hero != null && previewInitialized && ToBool(GetProp(template, "IsWeapon")))
                {
                    System.Type damageType = FindType("Awaken.TG.Main.Fights.DamageInfo.Damage");
                    if (damageType != null)
                    {
                        System.Reflection.MethodInfo calc = FindMethodByParameterType(damageType, "PreCalculateDealtDamage", 2, 1, "Awaken.TG.Main.Heroes.Items.Item");
                        if (calc != null)
                        {
                            object range = calc.Invoke(null, new object[] { hero, preview });
                            float minDamage = ToFloat(GetProp(range, "min"), 0f);
                            float maxDamage = ToFloat(GetProp(range, "max"), 0f);
                            if (minDamage != 0f || maxDamage != 0f)
                            {
                                sb.AppendLine();
                                sb.AppendLine(L("Расчетный урон для текущего персонажа (ур. предмета ") + _spawnLevel + "):");
                                sb.AppendLine(L("Урон: ") + minDamage.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + " - " + maxDamage.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                sb.AppendLine();
                                sb.AppendLine(L("Расчетный урон: игровой расчет вернул 0-0; ориентируйтесь на строку tooltip выше."));
                            }
                        }
                        System.Reflection.MethodInfo simple = FindMethodByParameterType(damageType, "GetDamageValueFromItemSimple", 2, 1, "Awaken.TG.Main.Heroes.Items.Item");
                        if (simple != null)
                        {
                            float simpleDamage = ToFloat(simple.Invoke(null, new object[] { hero, preview }), 0f);
                            if (simpleDamage != 0f) sb.AppendLine(L("Среднее значение: ") + simpleDamage.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            catch { }

            try
            {
                object stats = GetProp(preview, "ItemStats");
                if (stats != null)
                {
                    string statBlock = "";
                    object minStat = GetProp(stats, "BaseMinDmg");
                    object maxStat = GetProp(stats, "BaseMaxDmg");
                    float baseMin = StatModified(minStat);
                    float baseMax = StatModified(maxStat);
                    if (baseMin != 0f || baseMax != 0f) statBlock = statBlock + L("Базовый урон предмета: ") + baseMin.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + " - " + baseMax.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    object statObj = GetProp(stats, "Armor");
                    string statText = StatLine(L("Броня"), statObj);
                    if (statText != null) statBlock = statBlock + statText;
                    statObj = GetProp(stats, "Block");
                    statText = StatLine(L("Блок"), statObj);
                    if (statText != null) statBlock = statBlock + statText;
                    statObj = GetProp(stats, "ArmorPenetration");
                    statText = StatLine(L("Пробитие брони"), statObj);
                    if (statText != null) statBlock = statBlock + statText;
                    statObj = GetProp(stats, "ForceDamage");
                    statText = StatLine(L("Сила удара"), statObj);
                    if (statText != null) statBlock = statBlock + statText;
                    statObj = GetProp(stats, "PoiseDamage");
                    statText = StatLine(L("Урон стойкости"), statObj);
                    if (statText != null) statBlock = statBlock + statText;
                    if (statBlock.Length > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine(L("Дополнительные статы предмета:"));
                        sb.AppendLine(statBlock);
                    }
                }
            }
            catch { }
        }
        DiscardPreviewItem(preview);
        return sb.ToString();
    }

    static void SelectSpawnTemplate(object template)
    {
        _spawnTemplate = template;
        int minLevel = ToInt(GetProp(template, "MinimumItemLevel"), 1);
        if (_spawnLevel < minLevel) _spawnLevel = System.Math.Max(1, minLevel);
        _spawnPreview = BuildSpawnPreview(template);
        _lastAction = "Выбран предмет: " + TemplateName(template);
    }

    static void SpawnSelectedTemplate()
    {
        if (_spawnTemplate == null) { _lastAction = "Сначала выберите предмет"; return; }
        object hero = Hero();
        if (hero == null) { _lastAction = "Игрок не найден"; return; }
        object heroItems = GetProp(hero, "HeroItems");
        if (heroItems == null) { _lastAction = "HeroItems не найден"; return; }
        object item = CreateItemForTemplate(_spawnTemplate, _spawnQuantity, _spawnLevel);
        if (item == null) { _lastAction = "Не удалось создать предмет"; return; }
        try
        {
            System.Reflection.MethodInfo add = FindMethod(heroItems.GetType(), "Add", 2);
            if (add == null) add = FindMethod(heroItems.GetType(), "AddItemToInventory", 1);
            if (add == null) throw new System.Exception("Метод добавления в инвентарь не найден");
            if (add.GetParameters().Length == 2) add.Invoke(heroItems, new object[] { item, true });
            else add.Invoke(heroItems, new object[] { item });
            _lastAction = "Получено: " + TemplateName(_spawnTemplate) + " x" + _spawnQuantity + " (ур. " + _spawnLevel + ")";
        }
        catch (System.Exception ex) { _lastAction = "Ошибка выдачи предмета: " + ex.Message; }
    }

    static void InstallPatches()
    {
        Patch("Awaken.TG.Main.Character.HealthElement", "OnDamage", 1, "HealthOnDamagePrefix", null);
        Patch("Awaken.TG.Main.Heroes.Items.Item", "ChangeQuantity", 1, "ItemChangeQuantityPrefix", null);
        Patch("Awaken.TG.Main.Heroes.Items.Item", "get_Weight", 0, null, "ItemWeightPostfix");
        Patch("Awaken.TG.Main.Heroes.Items.ArmorWeight", "CalculateCurrentEquipmentWeight", 0, null, "EquipmentWeightPostfix");
        Patch("Awaken.TG.Main.Crafting.CraftingUtils", "IsRecipeCraftable", 2, "RecipeCraftablePrefix", null);
        Patch("Awaken.TG.Main.Crafting.Crafting", "DropIngredients", 1, "DropIngredientsPrefix", null);
        Patch("Awaken.TG.Main.Character.FallDamageUtil", "DealFallDamage", 2, "FallDamagePrefix", null);
        Patch("Awaken.TG.Main.Timing.GameRealTime", "WeatherIncrementSeconds", 1, "WeatherSecondsPrefix", null);
        Patch("Awaken.TG.Main.Heroes.Stats.Stat", "IncreaseBy", 2, "StatIncreasePrefix", null);
        Patch("Awaken.TG.Main.Character.ProficiencyStats", "TryAddXP", 2, "ProficiencyXpPrefix", null);
        Patch("Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.Slot.VItemsListElement", "get_Item", 0, null, "SelectedItemPostfix");
        Patch("Awaken.TG.Main.Heroes.CharacterSheet.Items.Panel.List.ItemsListElementUI", "get_Item", 0, null, "SelectedItemPostfix");
        Patch("Awaken.TG.Main.Heroes.Items.Tooltips.Descriptors.ExistingItemDescriptor", "get_Item", 0, null, "SelectedItemPostfix");
    }

    static void Patch(string typeName, string methodName, int parameterCount, string prefixName, string postfixName)
    {
        try
        {
            System.Type targetType = FindType(typeName);
            if (targetType == null) { _patchErrors.Add(typeName + "::" + methodName + " - тип не найден"); return; }
            System.Reflection.MethodInfo original = FindMethod(targetType, methodName, parameterCount);
            if (original == null) { _patchErrors.Add(typeName + "::" + methodName + " - метод не найден"); return; }
            HarmonyLib.HarmonyMethod pre = null;
            HarmonyLib.HarmonyMethod post = null;
            if (prefixName != null)
            {
                System.Reflection.MethodInfo pm = typeof(FoATrainerRuntime).GetMethod(prefixName, AllFlags);
                if (pm == null) { _patchErrors.Add(prefixName + " - prefix не найден"); return; }
                pre = new HarmonyLib.HarmonyMethod(pm);
            }
            if (postfixName != null)
            {
                System.Reflection.MethodInfo pm = typeof(FoATrainerRuntime).GetMethod(postfixName, AllFlags);
                if (pm == null) { _patchErrors.Add(postfixName + " - postfix не найден"); return; }
                post = new HarmonyLib.HarmonyMethod(pm);
            }
            _harmony.Patch(original, pre, post, null, null, null);
            _patchOk++;
        }
        catch (System.Exception ex)
        {
            _patchErrors.Add(typeName + "::" + methodName + " - " + ex.Message);
        }
    }

    static bool HealthOnDamagePrefix(object[] __args)
    {
        try
        {
            if (__args == null || __args.Length == 0 || __args[0] == null) return true;
            object damage = __args[0];
            object hero = Hero();
            if (hero == null) return true;
            object target = GetProp(damage, "Target");
            if (target == null) target = GetProp(damage, "TargetPure");
            object dealer = GetProp(damage, "DamageDealer");
            if (dealer == null) dealer = GetProp(damage, "DamageDealerPure");

            if (object.ReferenceEquals(target, hero))
            {
                if (GodMode) return false;
                if (DefenseMultiplierEnabled && DefenseMultiplier > 0.001f) ScaleDamage(damage, 1f / DefenseMultiplier);
            }
            if (object.ReferenceEquals(dealer, hero) && !object.ReferenceEquals(target, hero))
            {
                if (OneHitKills) SetDamageHuge(damage);
                else if (DamageMultiplierEnabled) ScaleDamage(damage, DamageMultiplier);
            }
        }
        catch (System.Exception ex)
        {
            _log.LogWarning("[FoATrainer] Damage patch: " + ex.Message);
        }
        return true;
    }

    static void ScaleDamage(object damage, float scale)
    {
        object raw = GetProp(damage, "RawData");
        if (raw == null) return;
        scale = Clamp(scale, 0f, 100000f);
        System.Reflection.FieldInfo finalF = FindField(raw.GetType(), "_finalCalculated");
        bool finalCalculated = finalF != null && ToBool(finalF.GetValue(raw));
        System.Reflection.FieldInfo calc = FindField(raw.GetType(), "_calculatedValue");
        System.Reflection.FieldInfo uncalc = FindField(raw.GetType(), "_uncalculatedValue");
        if (finalCalculated && calc != null)
        {
            calc.SetValue(raw, ToFloat(calc.GetValue(raw), 0f) * scale);
        }
        else if (uncalc != null)
        {
            uncalc.SetValue(raw, ToFloat(uncalc.GetValue(raw), 0f) * scale);
        }
        else if (calc != null)
        {
            calc.SetValue(raw, ToFloat(calc.GetValue(raw), 0f) * scale);
        }
    }

    static void SetDamageHuge(object damage)
    {
        object raw = GetProp(damage, "RawData");
        if (raw == null) return;
        System.Reflection.FieldInfo calc = FindField(raw.GetType(), "_calculatedValue");
        System.Reflection.FieldInfo uncalc = FindField(raw.GetType(), "_uncalculatedValue");
        if (calc != null) calc.SetValue(raw, 100000000f);
        if (uncalc != null) uncalc.SetValue(raw, 100000000f);
    }

    static bool ItemChangeQuantityPrefix(object __instance, ref bool __result, object[] __args)
    {
        try
        {
            if (!ItemsWontDecrease || __args == null || __args.Length == 0) return true;
            int amount = ToInt(__args[0], 0);
            if (amount < 0 && IsHeroItem(__instance))
            {
                __result = true;
                return false;
            }
        }
        catch { }
        return true;
    }

    static void ItemWeightPostfix(ref float __result)
    {
        if (ZeroItemWeight) __result = 0f;
    }

    static void EquipmentWeightPostfix(ref float __result)
    {
        if (ZeroEquipmentWeight) __result = 0f;
    }

    static bool RecipeCraftablePrefix(ref bool __result)
    {
        if (!IgnoreCraftingRequirement) return true;
        __result = true;
        return false;
    }

    static bool DropIngredientsPrefix()
    {
        return !IgnoreCraftingRequirement;
    }

    static bool FallDamagePrefix(object[] __args)
    {
        if (!NoFallDamage) return true;
        try
        {
            object hero = Hero();
            if (hero == null || __args == null || __args.Length == 0) return true;
            if (object.ReferenceEquals(__args[0], hero)) return false;
        }
        catch { }
        return true;
    }

    static bool WeatherSecondsPrefix(ref float __0)
    {
        if (FreezeDaytime) return false;
        if (TimePassSpeedEnabled) __0 *= Clamp(TimePassSpeed, 0f, 100f);
        return true;
    }

    static void StatIncreasePrefix(object __instance, ref float __0)
    {
        try
        {
            object hero = Hero();
            if (hero == null) return;
            object xp = GetProp(hero, "Experience");
            if (!object.ReferenceEquals(__instance, xp)) return;
            if (InfiniteExp)
            {
                float upper = StatUpper(xp);
                float current = StatBase(xp);
                float need = upper - current + 1f;
                if (need < 1f) need = 1f;
                if (__0 < need) __0 = need;
            }
            else if (ExpMultiplierEnabled)
            {
                __0 *= Clamp(ExpMultiplier, 0f, 10000f);
            }
        }
        catch { }
    }

    static void ProficiencyXpPrefix(ref float __1)
    {
        if (InfiniteProfExp)
        {
            if (__1 < 1000000f) __1 = 1000000f;
        }
        else if (ProfExpMultiplierEnabled)
        {
            __1 *= Clamp(ProfExpMultiplier, 0f, 10000f);
        }
    }

    static void SelectedItemPostfix(object __result)
    {
        if (__result == null || object.ReferenceEquals(__result, _selectedItem)) return;
        _selectedItem = __result;
        SelectedItemAmount = ToInt(GetProp(__result, "Quantity"), SelectedItemAmount);
        object level = GetProp(__result, "Level");
        if (level != null) SelectedItemLevel = (int)System.Math.Round(StatBase(level));
    }

    static System.Collections.IEnumerable HeroInventory()
    {
        object hero = Hero();
        if (hero == null) return null;
        object heroItems = GetProp(hero, "HeroItems");
        object inventory = GetProp(heroItems, "Inventory");
        return inventory as System.Collections.IEnumerable;
    }

    static bool IsHeroItem(object item)
    {
        if (item == null) return false;
        System.Collections.IEnumerable e = HeroInventory();
        if (e == null) return false;
        foreach (object it in e) if (object.ReferenceEquals(it, item)) return true;
        return false;
    }

    static void SetItemQuantity(object item, int amount)
    {
        if (item == null) return;
        amount = System.Math.Max(0, System.Math.Min(amount, 999999999));
        try
        {
            System.Reflection.PropertyInfo p = FindProperty(item.GetType(), "Quantity", true);
            if (p != null) { p.SetValue(item, amount, null); return; }
            System.Reflection.MethodInfo m = FindMethod(item.GetType(), "SetQuantity", 1);
            if (m != null) m.Invoke(item, new object[] { amount });
        }
        catch { }
    }

    static int SetInventoryCategoryAmount(int mode, int amount)
    {
        System.Collections.IEnumerable e = HeroInventory();
        if (e == null) return 0;
        int changed = 0;
        foreach (object item in e)
        {
            if (item == null) continue;
            object template = GetProp(item, "Template");
            if (template == null) continue;
            bool match = false;
            if (mode == 0) match = ToBool(GetProp(template, "IsPotion"));
            else if (mode == 1) match = ToBool(GetProp(template, "IsConsumable"));
            else if (mode == 2)
            {
                match = ToBool(GetProp(template, "IsComponent")) || ToBool(GetProp(template, "IsCrafting")) ||
                        ToBool(GetProp(template, "IsCookingComponent")) || ToBool(GetProp(template, "IsAlchemyComponent")) ||
                        ToBool(GetProp(template, "IsCraftingComponent"));
            }
            if (match)
            {
                SetItemQuantity(item, amount);
                changed++;
            }
        }
        return changed;
    }

    static void ApplyCobweb()
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(hero, "Cobweb"), CobwebValue);
        _lastAction = "Эфирная паутина: " + CobwebValue;
    }

    static void ApplyMoney()
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(hero, "Wealth"), MoneyValue);
        _lastAction = "Деньги: " + MoneyValue;
    }

    static void ApplyPotionAmount()
    {
        int n = SetInventoryCategoryAmount(0, PotionAmount);
        _lastAction = "Зелья: изменено стопок " + n;
    }

    static void ApplyConsumablesAmount()
    {
        int n = SetInventoryCategoryAmount(1, ConsumablesAmount);
        _lastAction = "Расходники: изменено стопок " + n;
    }

    static void ApplyMaterialsAmount()
    {
        int n = SetInventoryCategoryAmount(2, MaterialsAmount);
        _lastAction = "Материалы: изменено стопок " + n;
    }

    static void ApplySelectedAmount()
    {
        if (_selectedItem == null) { _lastAction = "Выбранный предмет не найден - наведите на предмет в инвентаре"; return; }
        SetItemQuantity(_selectedItem, SelectedItemAmount);
        _lastAction = "Количество выбранного предмета: " + SelectedItemAmount;
    }

    static void ApplySelectedLevel()
    {
        if (_selectedItem == null) { _lastAction = "Выбранный предмет не найден - наведите на предмет в инвентаре"; return; }
        object level = GetProp(_selectedItem, "Level");
        if (level == null) { _lastAction = "У выбранного предмета нет уровня"; return; }
        SetStatRaw(level, SelectedItemLevel);
        _lastAction = "Уровень выбранного предмета: " + SelectedItemLevel;
    }

    static void ApplyPlayerLevel()
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(hero, "Level"), PlayerLevel);
        _lastAction = "Уровень игрока: " + PlayerLevel;
    }

    static void ApplyAttributePoints()
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(HeroStats(hero), "BaseStatPoints"), AttributePoints);
        _lastAction = "Очки характеристик: " + AttributePoints;
    }

    static void ApplySkillPoints()
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(HeroStats(hero), "TalentPoints"), SkillPoints);
        _lastAction = "Очки навыков: " + SkillPoints;
    }

    static void ApplyRpgStat(string propertyName, int value, string label)
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(RPGStats(hero), propertyName), value);
        _lastAction = label + ": " + value;
    }

    static void ApplyProfStat(string propertyName, int value, string label)
    {
        object hero = Hero(); if (hero == null) return;
        SetStatRaw(GetProp(ProfStats(hero), propertyName), value);
        _lastAction = label + ": " + value;
    }

    static void SyncEditorsFromHero(object hero)
    {
        try
        {
            PlayerLevel = System.Math.Max(1, (int)System.Math.Round(StatBase(GetProp(hero, "Level"))));
            CobwebValue = (int)System.Math.Round(StatBase(GetProp(hero, "Cobweb")));
            MoneyValue = (int)System.Math.Round(StatBase(GetProp(hero, "Wealth")));
            object hs = HeroStats(hero);
            AttributePoints = (int)System.Math.Round(StatBase(GetProp(hs, "BaseStatPoints")));
            SkillPoints = (int)System.Math.Round(StatBase(GetProp(hs, "TalentPoints")));
            object r = RPGStats(hero);
            StrengthValue = ReadIntStat(r, "Strength", StrengthValue);
            EnduranceValue = ReadIntStat(r, "Endurance", EnduranceValue);
            DexterityValue = ReadIntStat(r, "Dexterity", DexterityValue);
            SpiritualityValue = ReadIntStat(r, "Spirituality", SpiritualityValue);
            PracticalityValue = ReadIntStat(r, "Practicality", PracticalityValue);
            PerceptionValue = ReadIntStat(r, "Perception", PerceptionValue);
            object p = ProfStats(hero);
            OneHandedValue = ReadIntStat(p, "OneHanded", OneHandedValue);
            TwoHandedValue = ReadIntStat(p, "TwoHanded", TwoHandedValue);
            UnarmedValue = ReadIntStat(p, "Unarmed", UnarmedValue);
            BlockingValue = ReadIntStat(p, "Shield", BlockingValue);
            AthleticsValue = ReadIntStat(p, "Athletics", AthleticsValue);
            LightArmorValue = ReadIntStat(p, "LightArmor", LightArmorValue);
            MediumArmorValue = ReadIntStat(p, "MediumArmor", MediumArmorValue);
            HeavyArmorValue = ReadIntStat(p, "HeavyArmor", HeavyArmorValue);
            ArcheryValue = ReadIntStat(p, "Archery", ArcheryValue);
            EvasionValue = ReadIntStat(p, "Evasion", EvasionValue);
            AgilityValue = ReadIntStat(p, "Acrobatics", AgilityValue);
            SneakValue = ReadIntStat(p, "Sneak", SneakValue);
            TheftValue = ReadIntStat(p, "Theft", TheftValue);
            MagicValue = ReadIntStat(p, "Magic", MagicValue);
            AlchemyValue = ReadIntStat(p, "Alchemy", AlchemyValue);
            CookingValue = ReadIntStat(p, "Cooking", CookingValue);
            HandcraftingValue = ReadIntStat(p, "Handcrafting", HandcraftingValue);
        }
        catch { }
    }

    static int ReadIntStat(object owner, string property, int fallback)
    {
        object s = GetProp(owner, property);
        if (s == null) return fallback;
        return (int)System.Math.Round(StatBase(s));
    }

    void HandleHotkeys()
    {
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Insert) || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F8)) ToggleMenuOnce();
        bool ctrl = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftControl) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightControl);
        bool alt = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftAlt) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightAlt);
        bool shift = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift);

        if (!ctrl && !alt && !shift && UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F6))
        {
            EspEnabled = !EspEnabled;
            if (!EspEnabled) _espEntries.Clear();
            _lastAction = EspEnabled ? "ESP включен" : "ESP выключен";
        }
        if (!ctrl && !alt && !shift && UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F7)) SetFlight(!FlightEnabled);

        if (shift)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F1)) ApplyPlayerLevel();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F2)) ApplyAttributePoints();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F3)) ApplySkillPoints();
        }

        if (ctrl && !alt)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad1)) ApplyCobweb();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad2)) ApplyMoney();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad3)) ZeroItemWeight = !ZeroItemWeight;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad4)) ZeroEquipmentWeight = !ZeroEquipmentWeight;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad5)) IgnoreCraftingRequirement = !IgnoreCraftingRequirement;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad6)) ApplyPotionAmount();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad7)) ApplyConsumablesAmount();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad8)) ApplyMaterialsAmount();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad9)) ApplySelectedAmount();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad0)) ApplySelectedLevel();

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha1)) ApplyRpgStat("Strength", StrengthValue, "Сила");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha2)) ApplyRpgStat("Endurance", EnduranceValue, "Выносливость");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha3)) ApplyRpgStat("Dexterity", DexterityValue, "Ловкость");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha4)) ApplyRpgStat("Spirituality", SpiritualityValue, "Духовность");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha5)) ApplyRpgStat("Practicality", PracticalityValue, "Практичность");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha6)) ApplyRpgStat("Perception", PerceptionValue, "Восприятие");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha7)) ApplyProfStat("OneHanded", OneHandedValue, "Одноручное");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha8)) ApplyProfStat("TwoHanded", TwoHandedValue, "Двуручное");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha9)) ApplyProfStat("Unarmed", UnarmedValue, "Без оружия");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha0)) ApplyProfStat("Shield", BlockingValue, "Блокирование");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Minus)) ApplyProfStat("Athletics", AthleticsValue, "Атлетика");
            return;
        }

        if (alt && !ctrl)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad1)) InfiniteExp = !InfiniteExp;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad2)) ExpMultiplierEnabled = !ExpMultiplierEnabled;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad3)) InfiniteProfExp = !InfiniteProfExp;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad4)) ProfExpMultiplierEnabled = !ProfExpMultiplierEnabled;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad5)) SetGameSpeed(!GameSpeedEnabled);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad6)) SetMovementSpeed(!MovementSpeedEnabled);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad7)) SetJumpHeight(!JumpHeightEnabled);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad8)) NoFallDamage = !NoFallDamage;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad9)) FreezeDaytime = !FreezeDaytime;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad0)) TimePassSpeedEnabled = !TimePassSpeedEnabled;

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha1)) ApplyProfStat("LightArmor", LightArmorValue, "Легкая броня");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha2)) ApplyProfStat("MediumArmor", MediumArmorValue, "Средняя броня");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha3)) ApplyProfStat("HeavyArmor", HeavyArmorValue, "Тяжелая броня");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha4)) ApplyProfStat("Archery", ArcheryValue, "Стрельба");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha5)) ApplyProfStat("Evasion", EvasionValue, "Уклонение");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha6)) ApplyProfStat("Acrobatics", AgilityValue, "Акробатика");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha7)) ApplyProfStat("Sneak", SneakValue, "Скрытность");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha8)) ApplyProfStat("Theft", TheftValue, "Воровство");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha9)) ApplyProfStat("Magic", MagicValue, "Магия");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Alpha0)) ApplyProfStat("Alchemy", AlchemyValue, "Алхимия");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Minus)) ApplyProfStat("Cooking", CookingValue, "Кулинария");
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Equals)) ApplyProfStat("Handcrafting", HandcraftingValue, "Ремесло");
            return;
        }

        if (!ctrl && !alt && !shift)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad1)) GodMode = !GodMode;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad2)) InfiniteHealth = !InfiniteHealth;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad3)) InfiniteMana = !InfiniteMana;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad4)) InfiniteStamina = !InfiniteStamina;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad5)) InfiniteKingsPower = !InfiniteKingsPower;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad6)) InfiniteOxygen = !InfiniteOxygen;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad7)) ItemsWontDecrease = !ItemsWontDecrease;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad8)) SetStealth(!StealthMode);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad9)) SetEasyLock(!EasyLockPicking);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Keypad0)) OneHitKills = !OneHitKills;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.KeypadPeriod)) DamageMultiplierEnabled = !DamageMultiplierEnabled;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.KeypadPlus)) DefenseMultiplierEnabled = !DefenseMultiplierEnabled;
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.KeypadDivide)) SetManaRate(!ManaRateEnabled);
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.KeypadMultiply)) SetStaminaRate(!StaminaRateEnabled);
        }
    }

    void ToggleMenuOnce()
    {
        int frame = UnityEngine.Time.frameCount;
        if (_lastMenuToggleFrame == frame) return;
        _lastMenuToggleFrame = frame;
        ToggleMenu();
    }

    void ToggleMenu()
    {
        _menuVisible = !_menuVisible;
        if (_menuVisible)
        {
            _savedCursorLock = UnityEngine.Cursor.lockState;
            _savedCursorVisible = UnityEngine.Cursor.visible;
            UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.GUI.FocusControl(null);
            UnityEngine.Cursor.lockState = _savedCursorLock;
            UnityEngine.Cursor.visible = _savedCursorVisible;
        }
    }

    void OnGUI()
    {
        UnityEngine.Event ev = UnityEngine.Event.current;
        if (ev != null && ev.type == UnityEngine.EventType.KeyDown &&
            (ev.keyCode == UnityEngine.KeyCode.Insert || ev.keyCode == UnityEngine.KeyCode.F8))
        {
            ToggleMenuOnce();
            ev.Use();
        }
        else if (ev != null && ev.type == UnityEngine.EventType.KeyDown && ev.keyCode == UnityEngine.KeyCode.F11 && _menuVisible)
        {
            ToggleFullscreen();
            ev.Use();
        }

        if (_windowStyle == null && (_menuVisible || EspEnabled)) BuildStyles();
        if (EspEnabled && ev != null && ev.type == UnityEngine.EventType.Repaint) DrawEspOverlay();
        if (!_menuVisible) return;

        if (!_windowPositioned)
        {
            float w = UnityEngine.Mathf.Min(1280f, UnityEngine.Screen.width - 30f);
            float h = UnityEngine.Mathf.Min(960f, UnityEngine.Screen.height - 30f);
            _windowRect = new UnityEngine.Rect((UnityEngine.Screen.width - w) * 0.5f, (UnityEngine.Screen.height - h) * 0.5f, w, h);
            _windowPositioned = true;
        }

        if (_windowFullscreen)
        {
            _windowRect = new UnityEngine.Rect(0f, 0f, UnityEngine.Screen.width, UnityEngine.Screen.height);
        }

        _windowRect = UnityEngine.GUI.Window(829421, _windowRect, DrawWindow, "", _windowStyle);
        if (_windowFullscreen)
        {
            _windowRect = new UnityEngine.Rect(0f, 0f, UnityEngine.Screen.width, UnityEngine.Screen.height);
        }
        else
        {
            ClampWindowToScreen();
        }
    }

    static UnityEngine.Texture2D SolidTexture(float r, float g, float b, float a)
    {
        UnityEngine.Texture2D t = new UnityEngine.Texture2D(1, 1);
        t.hideFlags = UnityEngine.HideFlags.HideAndDontSave;
        t.SetPixel(0, 0, new UnityEngine.Color(r, g, b, a));
        t.Apply();
        return t;
    }

    static void SetTextColor(UnityEngine.GUIStyle style, UnityEngine.Color color)
    {
        style.normal.textColor = color;
        style.hover.textColor = color;
        style.active.textColor = color;
        style.focused.textColor = color;
        style.onNormal.textColor = color;
        style.onHover.textColor = color;
        style.onActive.textColor = color;
        style.onFocused.textColor = color;
    }

    static void SetBackgrounds(UnityEngine.GUIStyle style, UnityEngine.Texture2D normal, UnityEngine.Texture2D hover, UnityEngine.Texture2D active)
    {
        style.normal.background = normal;
        style.hover.background = hover;
        style.active.background = active;
        style.focused.background = normal;
        style.onNormal.background = normal;
        style.onHover.background = hover;
        style.onActive.background = active;
        style.onFocused.background = normal;
    }

    static void BuildStyles()
    {
        UnityEngine.Color text = new UnityEngine.Color(0.94f, 0.95f, 0.97f, 1f);
        UnityEngine.Color muted = new UnityEngine.Color(0.62f, 0.67f, 0.73f, 1f);
        UnityEngine.Color accent = new UnityEngine.Color(0.82f, 0.64f, 0.34f, 1f);
        UnityEngine.Color darkText = new UnityEngine.Color(0.08f, 0.09f, 0.11f, 1f);

        // Fully opaque textures. Do not inherit the default Unity window/box focused states:
        // those states were the source of the white center / transparent edge effect.
        _texWindow = SolidTexture(0.040f, 0.050f, 0.066f, 1f);
        _texHeader = SolidTexture(0.070f, 0.085f, 0.110f, 1f);
        _texContent = SolidTexture(0.055f, 0.067f, 0.086f, 1f);
        _texCard = SolidTexture(0.085f, 0.102f, 0.130f, 1f);
        _texPanel = SolidTexture(0.090f, 0.108f, 0.138f, 1f);
        _texPanelAlt = SolidTexture(0.120f, 0.142f, 0.178f, 1f);
        _texAccent = SolidTexture(0.82f, 0.64f, 0.34f, 1f);
        _texAccentDark = SolidTexture(0.19f, 0.16f, 0.105f, 1f);
        _texInput = SolidTexture(0.030f, 0.039f, 0.052f, 1f);
        _texDanger = SolidTexture(0.48f, 0.145f, 0.16f, 1f);
        _texOrange = SolidTexture(0.94f, 0.42f, 0.075f, 1f);

        _windowStyle = new UnityEngine.GUIStyle();
        SetBackgrounds(_windowStyle, _texWindow, _texWindow, _texWindow);
        _windowStyle.padding = new UnityEngine.RectOffset(10, 10, 10, 10);
        _windowStyle.margin = new UnityEngine.RectOffset(0, 0, 0, 0);
        _windowStyle.border = new UnityEngine.RectOffset(0, 0, 0, 0);

        _headerStyle = new UnityEngine.GUIStyle();
        SetBackgrounds(_headerStyle, _texHeader, _texHeader, _texHeader);
        _headerStyle.padding = new UnityEngine.RectOffset(12, 12, 9, 9);
        _headerStyle.margin = new UnityEngine.RectOffset(0, 0, 0, 0);

        _contentStyle = new UnityEngine.GUIStyle();
        SetBackgrounds(_contentStyle, _texContent, _texContent, _texContent);
        _contentStyle.padding = new UnityEngine.RectOffset(8, 8, 7, 7);
        _contentStyle.margin = new UnityEngine.RectOffset(0, 0, 0, 0);

        _cardStyle = new UnityEngine.GUIStyle();
        SetBackgrounds(_cardStyle, _texCard, _texPanelAlt, _texCard);
        _cardStyle.padding = new UnityEngine.RectOffset(9, 9, 8, 8);
        _cardStyle.margin = new UnityEngine.RectOffset(0, 0, 2, 2);

        _titleStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _titleStyle.fontSize = 20;
        _titleStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _titleStyle.alignment = UnityEngine.TextAnchor.MiddleLeft;
        _titleStyle.richText = true;
        _titleStyle.wordWrap = false;
        _titleStyle.clipping = UnityEngine.TextClipping.Overflow;
        SetTextColor(_titleStyle, text);

        _subtitleStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _subtitleStyle.fontSize = 11;
        _subtitleStyle.alignment = UnityEngine.TextAnchor.MiddleLeft;
        SetTextColor(_subtitleStyle, muted);

        _sectionStyle = new UnityEngine.GUIStyle();
        _sectionStyle.fontSize = 12;
        _sectionStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _sectionStyle.padding = new UnityEngine.RectOffset(9, 9, 5, 5);
        SetBackgrounds(_sectionStyle, _texAccentDark, _texAccentDark, _texAccentDark);
        SetTextColor(_sectionStyle, accent);

        _statusStyle = new UnityEngine.GUIStyle();
        _statusStyle.wordWrap = true;
        _statusStyle.richText = true;
        _statusStyle.fontSize = 12;
        _statusStyle.padding = new UnityEngine.RectOffset(9, 9, 6, 6);
        SetBackgrounds(_statusStyle, _texPanel, _texPanel, _texPanel);
        SetTextColor(_statusStyle, text);

        _goodStatusStyle = new UnityEngine.GUIStyle(_statusStyle);
        _goodStatusStyle.fontStyle = UnityEngine.FontStyle.Bold;
        SetTextColor(_goodStatusStyle, new UnityEngine.Color(0.49f, 0.84f, 0.59f, 1f));

        _badStatusStyle = new UnityEngine.GUIStyle(_statusStyle);
        _badStatusStyle.fontStyle = UnityEngine.FontStyle.Bold;
        SetTextColor(_badStatusStyle, new UnityEngine.Color(0.94f, 0.43f, 0.43f, 1f));

        _tabStyle = new UnityEngine.GUIStyle();
        _tabStyle.fontSize = 11;
        _tabStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _tabStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _tabStyle.padding = new UnityEngine.RectOffset(7, 7, 7, 7);
        SetBackgrounds(_tabStyle, _texPanel, _texPanelAlt, _texAccentDark);
        SetTextColor(_tabStyle, text);

        _tabActiveStyle = new UnityEngine.GUIStyle(_tabStyle);
        SetBackgrounds(_tabActiveStyle, _texAccent, _texAccent, _texAccent);
        SetTextColor(_tabActiveStyle, darkText);

        _rowStyle = new UnityEngine.GUIStyle();
        SetBackgrounds(_rowStyle, _texPanel, _texPanelAlt, _texPanel);
        _rowStyle.padding = new UnityEngine.RectOffset(9, 9, 5, 5);
        _rowStyle.margin = new UnityEngine.RectOffset(0, 0, 1, 1);

        _toggleStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.toggle);
        _toggleStyle.fontSize = 12;
        _toggleStyle.alignment = UnityEngine.TextAnchor.MiddleLeft;
        SetTextColor(_toggleStyle, text);

        _toggleMarkerOnStyle = new UnityEngine.GUIStyle();
        _toggleMarkerOnStyle.fontSize = 11;
        _toggleMarkerOnStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _toggleMarkerOnStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _toggleMarkerOnStyle.padding = new UnityEngine.RectOffset(6, 6, 5, 5);
        SetBackgrounds(_toggleMarkerOnStyle, _texOrange, _texAccent, _texAccentDark);
        SetTextColor(_toggleMarkerOnStyle, darkText);

        _toggleMarkerOffStyle = new UnityEngine.GUIStyle();
        _toggleMarkerOffStyle.fontSize = 11;
        _toggleMarkerOffStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _toggleMarkerOffStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _toggleMarkerOffStyle.padding = new UnityEngine.RectOffset(6, 6, 5, 5);
        SetBackgrounds(_toggleMarkerOffStyle, _texInput, _texPanelAlt, _texPanel);
        SetTextColor(_toggleMarkerOffStyle, muted);

        _toggleLabelStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _toggleLabelStyle.fontSize = 12;
        _toggleLabelStyle.alignment = UnityEngine.TextAnchor.MiddleLeft;
        _toggleLabelStyle.padding = new UnityEngine.RectOffset(8, 5, 3, 3);
        SetTextColor(_toggleLabelStyle, text);

        _actionMarkerStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _actionMarkerStyle.fontSize = 15;
        _actionMarkerStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _actionMarkerStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        SetTextColor(_actionMarkerStyle, new UnityEngine.Color(0.94f, 0.42f, 0.075f, 1f));

        _actionLabelStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _actionLabelStyle.fontSize = 12;
        _actionLabelStyle.alignment = UnityEngine.TextAnchor.MiddleLeft;
        _actionLabelStyle.padding = new UnityEngine.RectOffset(4, 5, 3, 3);
        SetTextColor(_actionLabelStyle, text);

        _creatorLinkStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _creatorLinkStyle.fontSize = 11;
        _creatorLinkStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _creatorLinkStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _creatorLinkStyle.padding = new UnityEngine.RectOffset(2, 2, 1, 1);
        _creatorLinkStyle.wordWrap = false;
        _creatorLinkStyle.clipping = UnityEngine.TextClipping.Clip;
        SetTextColor(_creatorLinkStyle, new UnityEngine.Color(0.94f, 0.42f, 0.075f, 1f));
        _creatorLinkStyle.hover.textColor = accent;
        _creatorLinkStyle.active.textColor = accent;

        _textFieldStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.textField);
        _textFieldStyle.fontSize = 12;
        _textFieldStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _textFieldStyle.padding = new UnityEngine.RectOffset(7, 7, 5, 5);
        SetBackgrounds(_textFieldStyle, _texInput, _texInput, _texInput);
        SetTextColor(_textFieldStyle, text);

        _buttonStyle = new UnityEngine.GUIStyle();
        _buttonStyle.fontSize = 11;
        _buttonStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _buttonStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _buttonStyle.padding = new UnityEngine.RectOffset(7, 7, 5, 5);
        SetBackgrounds(_buttonStyle, _texAccent, _texAccent, _texAccentDark);
        SetTextColor(_buttonStyle, darkText);

        _dangerButtonStyle = new UnityEngine.GUIStyle(_buttonStyle);
        SetBackgrounds(_dangerButtonStyle, _texDanger, _texDanger, _texPanelAlt);
        SetTextColor(_dangerButtonStyle, text);

        _orangeButtonStyle = new UnityEngine.GUIStyle(_buttonStyle);
        SetBackgrounds(_orangeButtonStyle, _texOrange, _texAccent, _texAccentDark);
        SetTextColor(_orangeButtonStyle, darkText);

        _hotkeyStyle = new UnityEngine.GUIStyle();
        _hotkeyStyle.fontSize = 10;
        _hotkeyStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _hotkeyStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _hotkeyStyle.padding = new UnityEngine.RectOffset(5, 5, 4, 4);
        SetBackgrounds(_hotkeyStyle, _texInput, _texInput, _texInput);
        SetTextColor(_hotkeyStyle, accent);

        _footerStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _footerStyle.fontSize = 10;
        _footerStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        SetTextColor(_footerStyle, muted);

        _resizeGripStyle = new UnityEngine.GUIStyle(_hotkeyStyle);
        _resizeGripStyle.fontSize = 15;
        _resizeGripStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        SetBackgrounds(_resizeGripStyle, _texPanelAlt, _texAccentDark, _texAccent);

        _texEspBg = SolidTexture(0.02f, 0.025f, 0.035f, 0.72f);
        _espTextStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
        _espTextStyle.fontSize = 13;
        _espTextStyle.fontStyle = UnityEngine.FontStyle.Bold;
        _espTextStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
        _espTextStyle.wordWrap = false;
        _espTextStyle.clipping = UnityEngine.TextClipping.Overflow;
        _espTextStyle.padding = new UnityEngine.RectOffset(4, 4, 2, 2);
    }

    void ClampWindowToScreen()
    {
        float maxW = UnityEngine.Mathf.Max(520f, UnityEngine.Screen.width - 8f);
        float maxH = UnityEngine.Mathf.Max(420f, UnityEngine.Screen.height - 8f);
        float minW = UnityEngine.Mathf.Min(MinWindowWidth, maxW);
        float minH = UnityEngine.Mathf.Min(MinWindowHeight, maxH);
        _windowRect.width = UnityEngine.Mathf.Clamp(_windowRect.width, minW, maxW);
        _windowRect.height = UnityEngine.Mathf.Clamp(_windowRect.height, minH, maxH);
        float maxX = UnityEngine.Mathf.Max(0f, UnityEngine.Screen.width - _windowRect.width);
        float maxY = UnityEngine.Mathf.Max(0f, UnityEngine.Screen.height - _windowRect.height);
        _windowRect.x = UnityEngine.Mathf.Clamp(_windowRect.x, 0f, maxX);
        _windowRect.y = UnityEngine.Mathf.Clamp(_windowRect.y, 0f, maxY);
    }

    void ToggleFullscreen()
    {
        if (!_windowFullscreen)
        {
            _windowRectBeforeFullscreen = _windowRect;
            _windowFullscreen = true;
            _resizingWindow = false;
            _windowRect = new UnityEngine.Rect(0f, 0f, UnityEngine.Screen.width, UnityEngine.Screen.height);
            _lastAction = "Полноэкранный режим окна";
        }
        else
        {
            _windowFullscreen = false;
            _windowRect = _windowRectBeforeFullscreen;
            ClampWindowToScreen();
            _lastAction = "Оконный режим трейнера";
        }
    }

    void ChangeWindowSize(float dw, float dh)
    {
        if (_windowFullscreen) return;
        _windowRect.width += dw;
        _windowRect.height += dh;
        ClampWindowToScreen();
    }

    void ResetWindowSize()
    {
        if (_windowFullscreen) _windowFullscreen = false;
        float w = UnityEngine.Mathf.Min(1280f, UnityEngine.Screen.width - 30f);
        float h = UnityEngine.Mathf.Min(960f, UnityEngine.Screen.height - 30f);
        _windowRect.width = w;
        _windowRect.height = h;
        _windowRect.x = (UnityEngine.Screen.width - w) * 0.5f;
        _windowRect.y = (UnityEngine.Screen.height - h) * 0.5f;
        ClampWindowToScreen();
    }

    void HandleContinuousResize()
    {
        if (!_resizingWindow) return;
        if (_windowFullscreen || !UnityEngine.Input.GetMouseButton(0))
        {
            _resizingWindow = false;
            return;
        }

        UnityEngine.Vector2 mouse = new UnityEngine.Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Screen.height - UnityEngine.Input.mousePosition.y);
        float dx = mouse.x - _resizeStartMouseScreen.x;
        float dy = mouse.y - _resizeStartMouseScreen.y;
        float maxW = UnityEngine.Mathf.Max(MinWindowWidth, UnityEngine.Screen.width - _windowRect.x - 6f);
        float maxH = UnityEngine.Mathf.Max(MinWindowHeight, UnityEngine.Screen.height - _windowRect.y - 6f);
        _windowRect.width = UnityEngine.Mathf.Clamp(_resizeStartWindowSize.x + dx, MinWindowWidth, maxW);
        _windowRect.height = UnityEngine.Mathf.Clamp(_resizeStartWindowSize.y + dy, MinWindowHeight, maxH);
    }

    void HandleResizeGrip()
    {
        float visualSize = 24f;
        float hitSize = 48f;
        UnityEngine.Rect visual = new UnityEngine.Rect(_windowRect.width - visualSize - 5f, _windowRect.height - visualSize - 5f, visualSize, visualSize);
        UnityEngine.Rect hit = new UnityEngine.Rect(_windowRect.width - hitSize, _windowRect.height - hitSize, hitSize, hitSize);
        UnityEngine.GUI.Box(visual, "+", _resizeGripStyle);
        UnityEngine.Event e = UnityEngine.Event.current;
        if (e == null) return;
        if (e.type == UnityEngine.EventType.MouseDown && e.button == 0 && hit.Contains(e.mousePosition))
        {
            _resizingWindow = true;
            _resizeStartMouseScreen = new UnityEngine.Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Screen.height - UnityEngine.Input.mousePosition.y);
            _resizeStartWindowSize = new UnityEngine.Vector2(_windowRect.width, _windowRect.height);
            e.Use();
        }
    }

    static int ActiveCheatCount()
    {
        int c = 0;
        if (GodMode) c++; if (InfiniteHealth) c++; if (InfiniteMana) c++; if (InfiniteStamina) c++;
        if (InfiniteKingsPower) c++; if (InfiniteOxygen) c++; if (ItemsWontDecrease) c++; if (StealthMode) c++;
        if (EasyLockPicking) c++; if (OneHitKills) c++; if (DamageMultiplierEnabled) c++; if (DefenseMultiplierEnabled) c++;
        if (ManaRateEnabled) c++; if (StaminaRateEnabled) c++; if (ZeroItemWeight) c++; if (ZeroEquipmentWeight) c++;
        if (IgnoreCraftingRequirement) c++; if (InfiniteExp) c++; if (ExpMultiplierEnabled) c++; if (InfiniteProfExp) c++;
        if (ProfExpMultiplierEnabled) c++; if (GameSpeedEnabled) c++; if (MovementSpeedEnabled) c++; if (JumpHeightEnabled) c++;
        if (NoFallDamage) c++; if (FreezeDaytime) c++; if (TimePassSpeedEnabled) c++; if (FlightEnabled) c++;
        if (EspEnabled) c++; if (_weatherOverrideEnabled) c++;
        return c;
    }

    static int ActiveCheatCountForTab(int tab)
    {
        int c = 0;
        if (tab == 0)
        {
            if (GodMode) c++; if (InfiniteHealth) c++; if (InfiniteMana) c++; if (InfiniteStamina) c++;
            if (InfiniteKingsPower) c++; if (InfiniteOxygen) c++; if (ItemsWontDecrease) c++; if (StealthMode) c++;
            if (EasyLockPicking) c++; if (OneHitKills) c++; if (DamageMultiplierEnabled) c++; if (DefenseMultiplierEnabled) c++;
            if (ManaRateEnabled) c++; if (StaminaRateEnabled) c++; if (MovementSpeedEnabled) c++; if (JumpHeightEnabled) c++;
            if (NoFallDamage) c++; if (FlightEnabled) c++;
        }
        else if (tab == 1)
        {
            if (ZeroItemWeight) c++; if (ZeroEquipmentWeight) c++; if (IgnoreCraftingRequirement) c++;
        }
        else if (tab == 3)
        {
            if (InfiniteExp) c++; if (ExpMultiplierEnabled) c++; if (InfiniteProfExp) c++; if (ProfExpMultiplierEnabled) c++;
            if (GameSpeedEnabled) c++; if (FreezeDaytime) c++; if (TimePassSpeedEnabled) c++;
            if (_weatherOverrideEnabled) c++;
        }
        else if (tab == 5 && EspEnabled)
        {
            c++;
            if (EspItems) c++;
            if (EspContainers) c++;
            if (EspEnemies) c++;
            if (EspFriendlies) c++;
            if (EspNpcs) c++;
            if (EspMerchants) c++;
        }
        return c;
    }

    void DrawTabButton(string label, int tab)
    {
        UnityEngine.GUIStyle style = _tab == tab ? _tabActiveStyle : _tabStyle;
        string shown = L(label) + " (" + ActiveCheatCountForTab(tab) + ")";
        if (UnityEngine.GUILayout.Button(shown, style, UnityEngine.GUILayout.Height(32), UnityEngine.GUILayout.ExpandWidth(true))) _tab = tab;
    }

    int DrawFilterGrid(string[] labels, int selected, int columns)
    {
        if (labels == null || labels.Length == 0) return 0;
        if (columns < 1) columns = 1;
        for (int i = 0; i < labels.Length; i++)
        {
            if ((i % columns) == 0) UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUIStyle style = selected == i ? _tabActiveStyle : _tabStyle;
            if (UnityEngine.GUILayout.Button(L(labels[i]), style, UnityEngine.GUILayout.Height(27), UnityEngine.GUILayout.ExpandWidth(true))) selected = i;
            if ((i % columns) == columns - 1 || i == labels.Length - 1) UnityEngine.GUILayout.EndHorizontal();
        }
        return selected;
    }

    void DrawWindow(int id)
    {
        object hero = Hero();

        // Explicit opaque background prevents Unity skin focused/active states from flashing white.
        UnityEngine.GUI.DrawTexture(new UnityEngine.Rect(0f, 0f, _windowRect.width, _windowRect.height), _texWindow);
        UnityEngine.GUILayout.BeginVertical();

        UnityEngine.GUILayout.BeginVertical(_headerStyle);
        UnityEngine.GUILayout.BeginHorizontal(UnityEngine.GUILayout.Height(38));
        UnityEngine.GUILayout.BeginVertical(UnityEngine.GUILayout.Width(330));
        UnityEngine.GUILayout.Label("TAINTED GRAIL - TRAINER", _titleStyle, UnityEngine.GUILayout.Width(330), UnityEngine.GUILayout.Height(22));
        UnityEngine.GUILayout.Label("BepInEx v5 Mono  |  EN / RU  |  v2.6.4", _subtitleStyle, UnityEngine.GUILayout.Width(330), UnityEngine.GUILayout.Height(15));
        UnityEngine.GUILayout.EndVertical();
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label(L("Активно: ") + ActiveCheatCount(), _hotkeyStyle, UnityEngine.GUILayout.Width(88), UnityEngine.GUILayout.Height(28));
        if (UnityEngine.GUILayout.Button("-", _tabStyle, UnityEngine.GUILayout.Width(30), UnityEngine.GUILayout.Height(28))) ChangeWindowSize(-70f, -55f);
        if (UnityEngine.GUILayout.Button(L("СБРОС"), _tabStyle, UnityEngine.GUILayout.Width(62), UnityEngine.GUILayout.Height(28))) ResetWindowSize();
        if (UnityEngine.GUILayout.Button("+", _tabStyle, UnityEngine.GUILayout.Width(30), UnityEngine.GUILayout.Height(28))) ChangeWindowSize(70f, 55f);
        if (UnityEngine.GUILayout.Button(_windowFullscreen ? L("ОКНО") : L("ЭКРАН"), _tabStyle, UnityEngine.GUILayout.Width(58), UnityEngine.GUILayout.Height(28))) ToggleFullscreen();
        if (UnityEngine.GUILayout.Button(L("ВЫКЛ ВСЕ"), _orangeButtonStyle, UnityEngine.GUILayout.Width(100), UnityEngine.GUILayout.Height(28))) DisableAllFunctions();
        if (UnityEngine.GUILayout.Button("X", _dangerButtonStyle, UnityEngine.GUILayout.Width(32), UnityEngine.GUILayout.Height(28))) ToggleMenuOnce();
        UnityEngine.GUILayout.EndHorizontal();

        float creatorWidth = 310f;
        float creatorX = (_windowRect.width - creatorWidth) * 0.5f;
        if (UnityEngine.GUI.Button(new UnityEngine.Rect(creatorX, 8f, creatorWidth, 22f), "by Rijiy  |  Telegram - @Captain_S1ow", _creatorLinkStyle))
            UnityEngine.Application.OpenURL("https://t.me/Captain_S1ow");

        UnityEngine.GUILayout.Space(5);
        UnityEngine.GUILayout.BeginHorizontal();
        DrawTabButton("ИГРОК", 0);
        DrawTabButton("ИНВЕНТАРЬ", 1);
        DrawTabButton("ITEM SPAWNER", 2);
        DrawTabButton("ОПЫТ / ВРЕМЯ", 3);
        DrawTabButton("СТАТЫ", 4);
        DrawTabButton("ESP", 5);
        DrawTabButton("НАСТРОЙКИ", 6);
        UnityEngine.GUILayout.EndHorizontal();
        UnityEngine.GUILayout.EndVertical();

        UnityEngine.GUILayout.Space(6);
        if (hero == null)
            UnityEngine.GUILayout.Label(L("ИГРОК НЕ НАЙДЕН  |  загрузите сохранение"), _badStatusStyle, UnityEngine.GUILayout.Height(30));
        else
            UnityEngine.GUILayout.Label(L("ИГРОК НАЙДЕН  |  Последнее действие: ") + L(_lastAction), _goodStatusStyle, UnityEngine.GUILayout.Height(30));

        UnityEngine.GUILayout.Space(5);
        UnityEngine.GUILayout.BeginVertical(_contentStyle);
        _scroll = UnityEngine.GUILayout.BeginScrollView(_scroll);
        if (_tab == 0) DrawPlayerTab();
        else if (_tab == 1) DrawItemsTab();
        else if (_tab == 2) DrawSpawnerTab();
        else if (_tab == 3) DrawExpTimeTab();
        else if (_tab == 4) DrawStatsTab();
        else if (_tab == 5) DrawEspTab();
        else DrawDiagnosticsTab();
        UnityEngine.GUILayout.EndScrollView();
        UnityEngine.GUILayout.EndVertical();

        UnityEngine.GUILayout.Space(4);
        UnityEngine.GUILayout.Label(L("Insert / F8 - скрыть/показать  |  F6 - ESP  |  тяните правый нижний угол для изменения размера"), _footerStyle, UnityEngine.GUILayout.Height(18));
        UnityEngine.GUILayout.EndVertical();

        if (!_windowFullscreen) HandleResizeGrip();
        if (!_resizingWindow && !_windowFullscreen) UnityEngine.GUI.DragWindow(new UnityEngine.Rect(0f, 0f, UnityEngine.Mathf.Max(100f, _windowRect.width - 425f), 52f));
    }

    void DrawPlayerTab()
    {
        Section("Основные функции");
        GodMode = Toggle("Режим бога / игнорирование ударов", GodMode, "Num 1");
        InfiniteHealth = Toggle("Бесконечное здоровье", InfiniteHealth, "Num 2");
        InfiniteMana = Toggle("Бесконечная мана", InfiniteMana, "Num 3");
        InfiniteStamina = Toggle("Бесконечная выносливость", InfiniteStamina, "Num 4");
        InfiniteKingsPower = Toggle("Бесконечная сила короля", InfiniteKingsPower, "Num 5");
        InfiniteOxygen = Toggle("Бесконечный кислород", InfiniteOxygen, "Num 6");
        ItemsWontDecrease = Toggle("Предметы не уменьшаются", ItemsWontDecrease, "Num 7");
        bool s = Toggle("Режим скрытности", StealthMode, "Num 8"); if (s != StealthMode) SetStealth(s);
        bool l = Toggle("Простой взлом замков", EasyLockPicking, "Num 9"); if (l != EasyLockPicking) SetEasyLock(l);
        OneHitKills = Toggle("Сверхурон / убийство с одного удара", OneHitKills, "Num 0");

        Section("Урон и расход ресурсов");
        DamageMultiplierEnabled = ToggleFloat("Множитель урона", DamageMultiplierEnabled, ref DamageMultiplier, "Num .", 0f, 100000f, 0f, 100f);
        DefenseMultiplierEnabled = ToggleFloat("Множитель защиты", DefenseMultiplierEnabled, ref DefenseMultiplier, "Num +", 0.01f, 100000f, 0.01f, 100f);
        bool mr = ToggleFloat("Скорость расхода маны", ManaRateEnabled, ref ManaRate, "Num /", 0f, 100f, 0f, 5f); if (mr != ManaRateEnabled) SetManaRate(mr);
        bool sr = ToggleFloat("Скорость расхода выносливости", StaminaRateEnabled, ref StaminaRate, "Num *", 0f, 100f, 0f, 5f); if (sr != StaminaRateEnabled) SetStaminaRate(sr);

        Section("Передвижение и физика");
        bool ms = ToggleFloat("Скорость движения", MovementSpeedEnabled, ref MovementSpeed, "Alt+Num 6", 0.05f, 20f, 0.05f, 10f); if (ms != MovementSpeedEnabled) SetMovementSpeed(ms);
        bool jh = ToggleFloat("Высота прыжка", JumpHeightEnabled, ref JumpHeight, "Alt+Num 7", 0.05f, 20f, 0.05f, 10f); if (jh != JumpHeightEnabled) SetJumpHeight(jh);
        NoFallDamage = Toggle("Нет урона от падений", NoFallDamage, "Alt+Num 8");

        Section("Полет");
        bool fl = ToggleFloat("Полет / свободное перемещение", FlightEnabled, ref FlightSpeed, "F7", 0.1f, 200f, 0.1f, 60f); if (fl != FlightEnabled) SetFlight(fl);
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(36));
        float boostLabelWidth = UnityEngine.Mathf.Max(170f, _windowRect.width - 610f);
        UnityEngine.GUILayout.Label(L("◆"), _actionMarkerStyle, UnityEngine.GUILayout.Width(24), UnityEngine.GUILayout.Height(25));
        UnityEngine.GUILayout.Label(L("Ускорение полета (Shift)"), _actionLabelStyle, UnityEngine.GUILayout.Width(boostLabelWidth), UnityEngine.GUILayout.Height(25));
        NumericFloatField("FlightBoost", ref FlightBoost, 1f, 20f, 96f, 25f);
        NumericFloatSlider("FlightBoost", ref FlightBoost, 1f, 10f, 170f);
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label("Shift", _hotkeyStyle, UnityEngine.GUILayout.Width(105), UnityEngine.GUILayout.Height(25));
        UnityEngine.GUILayout.EndHorizontal();
        UnityEngine.GUILayout.Label(L("WASD - движение по направлению камеры, Space - вверх, Ctrl - вниз, Shift - ускорение. Полет отключает коллизии персонажа."), _statusStyle);
    }

    void DrawItemsTab()
    {
        Section("Валюта и вес");
        ActionInt("Эфирная паутина", ref CobwebValue, "Ctrl+Num 1", ApplyCobweb, 0, 999999999);
        ActionInt("Деньги", ref MoneyValue, "Ctrl+Num 2", ApplyMoney, 0, 999999999);
        ZeroItemWeight = Toggle("Нулевой вес предметов", ZeroItemWeight, "Ctrl+Num 3");
        ZeroEquipmentWeight = Toggle("Нулевой вес экипировки", ZeroEquipmentWeight, "Ctrl+Num 4");
        IgnoreCraftingRequirement = Toggle("Игнорировать требования крафта", IgnoreCraftingRequirement, "Ctrl+Num 5");

        Section("Количество предметов");
        ActionInt("Количество зелий", ref PotionAmount, "Ctrl+Num 6", ApplyPotionAmount, 0, 999999999);
        ActionInt("Количество расходников", ref ConsumablesAmount, "Ctrl+Num 7", ApplyConsumablesAmount, 0, 999999999);
        ActionInt("Количество материалов", ref MaterialsAmount, "Ctrl+Num 8", ApplyMaterialsAmount, 0, 999999999);
        ActionInt("Количество выбранного предмета", ref SelectedItemAmount, "Ctrl+Num 9", ApplySelectedAmount, 0, 999999999);
        ActionInt("Уровень выбранного предмета", ref SelectedItemLevel, "Ctrl+Num 0", ApplySelectedLevel, 0, 99999);
        UnityEngine.GUILayout.Label(_selectedItem == null ? L("Выбранный предмет: не определен. Наведите курсор на предмет в инвентаре.") : L("Выбранный предмет: найден"), _statusStyle);

        Section("Подсказка");
        UnityEngine.GUILayout.Label(L("Поиск, предпросмотр характеристик и выдача новых предметов теперь находятся на отдельной вкладке ITEM SPAWNER."), _statusStyle);
    }

    void DrawSpawnerTab()
    {
        Section("Фильтры предметов");
        UnityEngine.GUILayout.BeginVertical(_cardStyle);
        UnityEngine.GUILayout.Label(L("ГРУППА"), _sectionStyle, UnityEngine.GUILayout.Height(24));
        int oldGroup = _itemGroup;
        int groupColumns = _windowRect.width >= 1120f ? 7 : 5;
        _itemGroup = DrawFilterGrid(_itemGroupNames, _itemGroup, groupColumns);
        if (_itemGroup != oldGroup)
        {
            _itemSubtype = 0;
            _itemDetail = 0;
            _spawnListScroll = UnityEngine.Vector2.zero;
        }
        string[] subtypeNames = ItemSubtypeNames(_itemGroup);
        if (subtypeNames.Length > 1)
        {
            UnityEngine.GUILayout.Space(5);
            UnityEngine.GUILayout.Label(L("ТИП"), _sectionStyle, UnityEngine.GUILayout.Height(24));
            int subtypeColumns = _windowRect.width >= 1120f ? 6 : 4;
            int oldSubtype = _itemSubtype;
            _itemSubtype = DrawFilterGrid(subtypeNames, _itemSubtype, subtypeColumns);
            if (_itemSubtype != oldSubtype) _itemDetail = 0;
            string[] detailNames = ItemDetailNames(_itemGroup, _itemSubtype);
            if (detailNames.Length > 1)
            {
                UnityEngine.GUILayout.Space(5);
                UnityEngine.GUILayout.Label(L("ПОДТИП"), _sectionStyle, UnityEngine.GUILayout.Height(24));
                _itemDetail = DrawFilterGrid(detailNames, _itemDetail, 4);
            }
            else
            {
                _itemDetail = 0;
            }
        }
        else
        {
            _itemSubtype = 0;
            _itemDetail = 0;
        }
        UnityEngine.GUILayout.EndVertical();

        Section("Поиск");
        UnityEngine.GUILayout.BeginHorizontal(_cardStyle, UnityEngine.GUILayout.Height(42));
        UnityEngine.GUILayout.Label(L("Название"), _actionLabelStyle, UnityEngine.GUILayout.Width(72), UnityEngine.GUILayout.Height(27));
        _itemSearch = UnityEngine.GUILayout.TextField(_itemSearch, _textFieldStyle, UnityEngine.GUILayout.Height(27));
        if (UnityEngine.GUILayout.Button(_itemTemplatesLoaded ? L("ОБНОВИТЬ БАЗУ") : L("ЗАГРУЗИТЬ БАЗУ"), _buttonStyle, UnityEngine.GUILayout.Width(145), UnityEngine.GUILayout.Height(27))) LoadItemTemplates();
        UnityEngine.GUILayout.EndHorizontal();
        _showHiddenItems = Toggle("Показывать скрытые / служебные предметы", _showHiddenItems, "");
        string filterPath = L(_itemGroupNames[_itemGroup]);
        if (subtypeNames.Length > 1 && _itemSubtype >= 0 && _itemSubtype < subtypeNames.Length && _itemSubtype > 0) filterPath = filterPath + " > " + L(subtypeNames[_itemSubtype]);
        string[] activeDetailNames = ItemDetailNames(_itemGroup, _itemSubtype);
        if (activeDetailNames.Length > 1 && _itemDetail > 0 && _itemDetail < activeDetailNames.Length) filterPath = filterPath + " > " + L(activeDetailNames[_itemDetail]);
        UnityEngine.GUILayout.Label(L(_itemTemplatesStatus) + L("  |  Фильтр: ") + filterPath, _statusStyle);

        float leftWidth = UnityEngine.Mathf.Clamp((_windowRect.width - 54f) * 0.43f, 300f, 500f);
        float listHeight = UnityEngine.Mathf.Max(220f, _windowRect.height - 520f);

        UnityEngine.GUILayout.BeginHorizontal();
        UnityEngine.GUILayout.BeginVertical(_cardStyle, UnityEngine.GUILayout.Width(leftWidth));
        UnityEngine.GUILayout.Label(L("РЕЗУЛЬТАТЫ"), _sectionStyle, UnityEngine.GUILayout.Height(27));
        if (!_itemTemplatesLoaded)
        {
            UnityEngine.GUILayout.Label(L("Нажмите 'ЗАГРУЗИТЬ БАЗУ'. После этого можно выбрать группу, тип и искать только внутри выбранной категории."), _statusStyle);
        }
        else
        {
            int matches = 0;
            int shown = 0;
            string q = _itemSearch == null ? "" : _itemSearch.Trim();
            _spawnListScroll = UnityEngine.GUILayout.BeginScrollView(_spawnListScroll, UnityEngine.GUILayout.Height(listHeight));
            for (int i = 0; i < _itemTemplates.Count; i++)
            {
                object t = _itemTemplates[i];
                if (!_showHiddenItems && ToBool(GetProp(t, "HiddenOnUI"))) continue;
                if (!MatchesItemFilter(t)) continue;
                if (q.Length > 0 && TemplateSearchBlob(t).IndexOf(q, System.StringComparison.OrdinalIgnoreCase) < 0) continue;
                matches++;
                if (shown >= 120) continue;
                shown++;
                bool selected = object.ReferenceEquals(_spawnTemplate, t);
                UnityEngine.GUILayout.BeginVertical(selected ? _contentStyle : _rowStyle);
                if (UnityEngine.GUILayout.Button(TemplateName(t), selected ? _tabActiveStyle : _tabStyle, UnityEngine.GUILayout.Height(29))) SelectSpawnTemplate(t);
                UnityEngine.GUILayout.BeginHorizontal();
                UnityEngine.GUILayout.Label(L(TemplateDetailedKind(t)), _hotkeyStyle, UnityEngine.GUILayout.Height(23), UnityEngine.GUILayout.ExpandWidth(true));
                UnityEngine.GUILayout.FlexibleSpace();
                UnityEngine.GUILayout.EndHorizontal();
                UnityEngine.GUILayout.EndVertical();
            }
            UnityEngine.GUILayout.EndScrollView();
            UnityEngine.GUILayout.Label(L("Найдено: ") + matches + (matches > 120 ? L("  |  показаны первые 120 - уточните фильтр или поиск") : ""), _footerStyle, UnityEngine.GUILayout.Height(20));
        }
        UnityEngine.GUILayout.EndVertical();

        UnityEngine.GUILayout.Space(8);
        UnityEngine.GUILayout.BeginVertical(_cardStyle);
        UnityEngine.GUILayout.Label(L("КАРТОЧКА ПРЕДМЕТА"), _sectionStyle, UnityEngine.GUILayout.Height(27));
        if (_spawnTemplate == null)
        {
            UnityEngine.GUILayout.Label(L("Выберите предмет слева. Для оружия урон будет рассчитан игровыми методами с учетом текущего персонажа и выбранного уровня предмета."), _statusStyle, UnityEngine.GUILayout.MinHeight(120));
        }
        else
        {
            UnityEngine.GUILayout.Label(TemplateName(_spawnTemplate), _titleStyle, UnityEngine.GUILayout.Height(28));
            UnityEngine.GUILayout.Label(L(TemplateDetailedKind(_spawnTemplate)), _subtitleStyle, UnityEngine.GUILayout.Height(18));
            UnityEngine.GUILayout.Space(5);

            UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(39));
            UnityEngine.GUILayout.Label(L("Кол-во"), _actionLabelStyle, UnityEngine.GUILayout.Width(55));
            NumericIntField("SpawnQuantity", ref _spawnQuantity, 1, 999999, 75f, 27f);
            UnityEngine.GUILayout.Label(L("Уровень"), _actionLabelStyle, UnityEngine.GUILayout.Width(62));
            int oldLevel = _spawnLevel;
            NumericIntField("SpawnLevel", ref _spawnLevel, 1, 9999, 75f, 27f);
            if (oldLevel != _spawnLevel) _spawnPreview = BuildSpawnPreview(_spawnTemplate);
            UnityEngine.GUILayout.EndHorizontal();

            UnityEngine.GUILayout.BeginHorizontal();
            if (UnityEngine.GUILayout.Button(L("ОБНОВИТЬ СТАТЫ"), _tabStyle, UnityEngine.GUILayout.Height(30))) _spawnPreview = BuildSpawnPreview(_spawnTemplate);
            if (UnityEngine.GUILayout.Button(L("ПОЛУЧИТЬ ПРЕДМЕТ"), _buttonStyle, UnityEngine.GUILayout.Height(30))) SpawnSelectedTemplate();
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(5);
            UnityEngine.GUILayout.Label(_spawnPreview, _statusStyle, UnityEngine.GUILayout.MinHeight(UnityEngine.Mathf.Max(180f, listHeight - 95f)));
        }
        UnityEngine.GUILayout.EndVertical();
        UnityEngine.GUILayout.EndHorizontal();
    }

    void DrawExpTimeTab()
    {
        Section("Опыт");
        InfiniteExp = Toggle("Бесконечный опыт", InfiniteExp, "Alt+Num 1");
        ExpMultiplierEnabled = ToggleFloat("Множитель опыта", ExpMultiplierEnabled, ref ExpMultiplier, "Alt+Num 2", 0f, 10000f, 0f, 100f);
        InfiniteProfExp = Toggle("Бесконечный опыт мастерства", InfiniteProfExp, "Alt+Num 3");
        ProfExpMultiplierEnabled = ToggleFloat("Множитель опыта мастерства", ProfExpMultiplierEnabled, ref ProfExpMultiplier, "Alt+Num 4", 0f, 10000f, 0f, 100f);

        Section("Скорость игры и время");
        bool gs = ToggleFloat("Скорость игры", GameSpeedEnabled, ref GameSpeed, "Alt+Num 5", 0.05f, 20f, 0.05f, 5f); if (gs != GameSpeedEnabled) SetGameSpeed(gs);
        FreezeDaytime = Toggle("Заморозить время суток", FreezeDaytime, "Alt+Num 9");
        TimePassSpeedEnabled = ToggleFloat("Скорость течения времени", TimePassSpeedEnabled, ref TimePassSpeed, "Alt+Num 0", 0f, 100f, 0f, 10f);

        DrawWeatherSection();
    }

    void DrawWeatherSection()
    {
        Section("Погода");
        bool forced = Toggle("Принудительная погода", _weatherOverrideEnabled, "");
        if (forced != _weatherOverrideEnabled) SetWeatherOverride(forced);

        UnityEngine.GUILayout.BeginHorizontal(_cardStyle, UnityEngine.GUILayout.Height(40));
        UnityEngine.GUILayout.Label(L("Выбор погоды"), _actionLabelStyle, UnityEngine.GUILayout.Width(190), UnityEngine.GUILayout.Height(27));
        string selected = _weatherOverrideEnabled ? WeatherPresetName(_selectedWeatherPreset) : L("Автоматически / По умолчанию");
        if (UnityEngine.GUILayout.Button(selected, _buttonStyle, UnityEngine.GUILayout.Height(27))) _weatherDropdownOpen = !_weatherDropdownOpen;
        UnityEngine.GUILayout.EndHorizontal();

        if (_weatherDropdownOpen)
        {
            UnityEngine.GUILayout.BeginVertical(_cardStyle);
            if (UnityEngine.GUILayout.Button(L("Автоматически / По умолчанию"), !_weatherOverrideEnabled ? _tabActiveStyle : _tabStyle, UnityEngine.GUILayout.Height(28)))
            {
                SetWeatherOverride(false);
            }
            int columns = _windowRect.width >= 1050f ? 3 : 2;
            for (int i = 0; i < _weatherPresetNames.Count; i += columns)
            {
                UnityEngine.GUILayout.BeginHorizontal();
                for (int c = 0; c < columns; c++)
                {
                    int index = i + c;
                    if (index < _weatherPresetNames.Count)
                    {
                        bool active = _weatherOverrideEnabled && index == _selectedWeatherPreset;
                        if (UnityEngine.GUILayout.Button(_weatherPresetNames[index], active ? _tabActiveStyle : _tabStyle, UnityEngine.GUILayout.Height(28))) SelectWeatherPreset(index);
                    }
                    else UnityEngine.GUILayout.FlexibleSpace();
                }
                UnityEngine.GUILayout.EndHorizontal();
            }
            UnityEngine.GUILayout.EndVertical();
        }

        string currentName = WeatherPresetName(_weatherCurrentIndex);
        string details;
        if (_weatherController == null)
        {
            details = Language == 0 ? "Weather system is not loaded yet. Load a save or enter a location." : "Погодная система еще не загружена. Загрузите сохранение или войдите в локацию.";
        }
        else if (Language == 0)
        {
            details = "Current: " + currentName + "  |  Precipitation: " + _weatherPrecipitationIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + "  |  Rain: " + _weatherRainIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + "  |  Snow: " + _weatherSnowIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + (_weatherHeavyRain ? "  |  Heavy rain" : "");
        }
        else
        {
            details = "Текущая: " + currentName + "  |  Осадки: " + _weatherPrecipitationIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + "  |  Дождь: " + _weatherRainIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + "  |  Снег: " + _weatherSnowIntensity.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                + (_weatherHeavyRain ? "  |  Сильный дождь" : "");
        }
        UnityEngine.GUILayout.Label(details, _statusStyle);
        UnityEngine.GUILayout.Label(L("Используются только штатные пресеты и переходы игры. Автоматический режим возвращает управление игре."), _footerStyle);
    }

    void DrawStatsTab()
    {
        Section("Игрок");
        ActionInt("Уровень игрока", ref PlayerLevel, "Shift+F1", ApplyPlayerLevel, 1, 9999);
        ActionInt("Очки характеристик", ref AttributePoints, "Shift+F2", ApplyAttributePoints, 0, 999999);
        ActionInt("Очки навыков", ref SkillPoints, "Shift+F3", ApplySkillPoints, 0, 999999);

        Section("Характеристики");
        ActionInt("Сила", ref StrengthValue, "Ctrl+1", delegate { ApplyRpgStat("Strength", StrengthValue, "Сила"); }, 0, 9999);
        ActionInt("Выносливость", ref EnduranceValue, "Ctrl+2", delegate { ApplyRpgStat("Endurance", EnduranceValue, "Выносливость"); }, 0, 9999);
        ActionInt("Ловкость", ref DexterityValue, "Ctrl+3", delegate { ApplyRpgStat("Dexterity", DexterityValue, "Ловкость"); }, 0, 9999);
        ActionInt("Духовность", ref SpiritualityValue, "Ctrl+4", delegate { ApplyRpgStat("Spirituality", SpiritualityValue, "Духовность"); }, 0, 9999);
        ActionInt("Практичность", ref PracticalityValue, "Ctrl+5", delegate { ApplyRpgStat("Practicality", PracticalityValue, "Практичность"); }, 0, 9999);
        ActionInt("Восприятие", ref PerceptionValue, "Ctrl+6", delegate { ApplyRpgStat("Perception", PerceptionValue, "Восприятие"); }, 0, 9999);

        Section("Мастерство");
        ActionInt("Одноручное оружие", ref OneHandedValue, "Ctrl+7", delegate { ApplyProfStat("OneHanded", OneHandedValue, "Одноручное"); }, 0, 9999);
        ActionInt("Двуручное оружие", ref TwoHandedValue, "Ctrl+8", delegate { ApplyProfStat("TwoHanded", TwoHandedValue, "Двуручное"); }, 0, 9999);
        ActionInt("Без оружия", ref UnarmedValue, "Ctrl+9", delegate { ApplyProfStat("Unarmed", UnarmedValue, "Без оружия"); }, 0, 9999);
        ActionInt("Блокирование", ref BlockingValue, "Ctrl+0", delegate { ApplyProfStat("Shield", BlockingValue, "Блокирование"); }, 0, 9999);
        ActionInt("Атлетика", ref AthleticsValue, "Ctrl+-", delegate { ApplyProfStat("Athletics", AthleticsValue, "Атлетика"); }, 0, 9999);
        ActionInt("Легкая броня", ref LightArmorValue, "Alt+1", delegate { ApplyProfStat("LightArmor", LightArmorValue, "Легкая броня"); }, 0, 9999);
        ActionInt("Средняя броня", ref MediumArmorValue, "Alt+2", delegate { ApplyProfStat("MediumArmor", MediumArmorValue, "Средняя броня"); }, 0, 9999);
        ActionInt("Тяжелая броня", ref HeavyArmorValue, "Alt+3", delegate { ApplyProfStat("HeavyArmor", HeavyArmorValue, "Тяжелая броня"); }, 0, 9999);
        ActionInt("Стрельба", ref ArcheryValue, "Alt+4", delegate { ApplyProfStat("Archery", ArcheryValue, "Стрельба"); }, 0, 9999);
        ActionInt("Уклонение", ref EvasionValue, "Alt+5", delegate { ApplyProfStat("Evasion", EvasionValue, "Уклонение"); }, 0, 9999);
        ActionInt("Ловкость / акробатика", ref AgilityValue, "Alt+6", delegate { ApplyProfStat("Acrobatics", AgilityValue, "Акробатика"); }, 0, 9999);
        ActionInt("Скрытность", ref SneakValue, "Alt+7", delegate { ApplyProfStat("Sneak", SneakValue, "Скрытность"); }, 0, 9999);
        ActionInt("Воровство", ref TheftValue, "Alt+8", delegate { ApplyProfStat("Theft", TheftValue, "Воровство"); }, 0, 9999);
        ActionInt("Магия", ref MagicValue, "Alt+9", delegate { ApplyProfStat("Magic", MagicValue, "Магия"); }, 0, 9999);
        ActionInt("Алхимия", ref AlchemyValue, "Alt+0", delegate { ApplyProfStat("Alchemy", AlchemyValue, "Алхимия"); }, 0, 9999);
        ActionInt("Кулинария", ref CookingValue, "Alt+-", delegate { ApplyProfStat("Cooking", CookingValue, "Кулинария"); }, 0, 9999);
        ActionInt("Ремесло", ref HandcraftingValue, "Alt++", delegate { ApplyProfStat("Handcrafting", HandcraftingValue, "Ремесло"); }, 0, 9999);
    }

    static void SettingFloat(string label, ref float value, float min, float max, string suffix)
    {
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(38));
        UnityEngine.GUILayout.Label("◆", _actionMarkerStyle, UnityEngine.GUILayout.Width(24), UnityEngine.GUILayout.Height(27));
        float labelWidth = UnityEngine.Mathf.Max(170f, _windowRect.width - 610f);
        UnityEngine.GUILayout.Label(L(label), _actionLabelStyle, UnityEngine.GUILayout.Width(labelWidth), UnityEngine.GUILayout.Height(27));
        NumericFloatField("SettingFloat." + label, ref value, min, max, 90f, 27f);
        NumericFloatSlider("SettingFloat." + label, ref value, min, max, 180f);
        UnityEngine.GUILayout.Label(suffix, _hotkeyStyle, UnityEngine.GUILayout.Width(45), UnityEngine.GUILayout.Height(27));
        UnityEngine.GUILayout.EndHorizontal();
    }

    static void SettingInt(string label, ref int value, int min, int max)
    {
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(38));
        UnityEngine.GUILayout.Label("◆", _actionMarkerStyle, UnityEngine.GUILayout.Width(24), UnityEngine.GUILayout.Height(27));
        float labelWidth = UnityEngine.Mathf.Max(170f, _windowRect.width - 565f);
        UnityEngine.GUILayout.Label(L(label), _actionLabelStyle, UnityEngine.GUILayout.Width(labelWidth), UnityEngine.GUILayout.Height(27));
        NumericIntField("SettingInt." + label, ref value, min, max, 90f, 27f);
        NumericIntSlider("SettingInt." + label, ref value, min, max, 180f);
        UnityEngine.GUILayout.EndHorizontal();
    }

    void DrawEspTab()
    {
        Section("ESP");
        bool master = Toggle("Общий ESP", EspEnabled, "F6");
        if (master != EspEnabled)
        {
            EspEnabled = master;
            if (!EspEnabled) _espEntries.Clear();
            else _espNextScan = 0f;
        }
        UnityEngine.GUILayout.Label(L("ESP продолжает отображаться, когда меню трейнера скрыто."), _statusStyle);

        Section("Объекты ESP");
        EspItems = Toggle("Предметы", EspItems, "");
        EspContainers = Toggle("Контейнеры", EspContainers, "");
        EspEnemies = Toggle("Враждебные NPC / враги", EspEnemies, "");
        EspFriendlies = Toggle("Дружественные NPC / союзники", EspFriendlies, "");
        EspNpcs = Toggle("Нейтральные NPC", EspNpcs, "");
        EspMerchants = Toggle("Торговцы", EspMerchants, "");
        EspShowDead = Toggle("Показывать мертвых NPC / врагов", EspShowDead, "");
        EspShowLootState = Toggle("Статус контейнеров / трупов", EspShowLootState, "");
        EspHideEmptyLoot = Toggle("Скрывать пустые контейнеры / трупы", EspHideEmptyLoot, "");

        Section("Фильтр предметов ESP");
        EspShowItemWeapons = Toggle("Оружие", EspShowItemWeapons, "");
        EspShowItemArmor = Toggle("Броня и щиты", EspShowItemArmor, "");
        EspShowItemConsumables = Toggle("Расходники", EspShowItemConsumables, "");
        EspShowItemMaterials = Toggle("Материалы", EspShowItemMaterials, "");
        EspShowItemImportant = Toggle("Важные / ключевые предметы", EspShowItemImportant, "");
        EspShowItemOther = Toggle("Прочие предметы", EspShowItemOther, "");

        Section("Дальность ESP");
        SettingFloat("Предметы - дальность", ref EspItemDistance, 5f, 1000f, "m");
        SettingFloat("Контейнеры - дальность", ref EspContainerDistance, 5f, 1000f, "m");
        SettingFloat("Враги - дальность", ref EspEnemyDistance, 5f, 2000f, "m");
        SettingFloat("NPC / союзники / торговцы - дальность", ref EspNpcDistance, 5f, 2000f, "m");

        Section("Отображение ESP");
        EspShowNames = Toggle("Название", EspShowNames, "");
        EspShowDistance = Toggle("Расстояние", EspShowDistance, "");
        EspShowHealth = Toggle("HP существ / NPC", EspShowHealth, "");
        EspShowHealthBars = Toggle("Полоски HP", EspShowHealthBars, "");
        SettingInt("Ширина полоски HP", ref EspHealthBarWidth, 8, 120);
        SettingInt("Высота полоски HP", ref EspHealthBarHeight, 1, 8);
        EspShowIcons = Toggle("Иконки ESP", EspShowIcons, "");
        EspIconsOnly = Toggle("Только иконки (без названий)", EspIconsOnly, "");
        SettingInt("Размер иконок ESP", ref EspIconSize, 6, 42);
        EspShowBackground = Toggle("Темный фон подписи", EspShowBackground, "");
        SettingInt("Размер текста ESP", ref EspFontSize, 5, 24);
        SettingInt("Максимум подписей на экране", ref EspMaxObjects, 10, 500);
        SettingFloat("Интервал сканирования", ref EspScanInterval, 0.50f, 3.0f, "sec");

        Section("Статус ESP");
        UnityEngine.GUILayout.Label((Language == 1 ? "В кеше: " : "Cached: ") + _espEntries.Count + (Language == 1 ? "  |  На экране: " : "  |  On screen: ") + _espVisibleLastFrame + "  |  Camera: " + _espCameraName, _statusStyle);
        UnityEngine.GUILayout.Label(L(_espStatus), _statusStyle);
        UnityEngine.GUILayout.Label(L("Цвета: враги - красный, союзники - зеленый, нейтральные - желтый, торговцы - фиолетовый."), _statusStyle);
        if (UnityEngine.GUILayout.Button(L("ПЕРЕСКАНИРОВАТЬ ESP"), _buttonStyle, UnityEngine.GUILayout.Height(30))) _espNextScan = 0f;
    }

    void DrawDiagnosticsTab()
    {
        Section("Локализация");
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(40));
        UnityEngine.GUILayout.Label(L("Язык интерфейса"), _actionLabelStyle, UnityEngine.GUILayout.Width(180), UnityEngine.GUILayout.Height(27));
        UnityEngine.GUIStyle enStyle = Language == 0 ? _tabActiveStyle : _tabStyle;
        UnityEngine.GUIStyle ruStyle = Language == 1 ? _tabActiveStyle : _tabStyle;
        if (UnityEngine.GUILayout.Button("ENGLISH", enStyle, UnityEngine.GUILayout.Width(120), UnityEngine.GUILayout.Height(27)))
        {
            Language = 0;
            if (_spawnTemplate != null) _spawnPreview = BuildSpawnPreview(_spawnTemplate);
        }
        if (UnityEngine.GUILayout.Button("РУССКИЙ", ruStyle, UnityEngine.GUILayout.Width(120), UnityEngine.GUILayout.Height(27)))
        {
            Language = 1;
            if (_spawnTemplate != null) _spawnPreview = BuildSpawnPreview(_spawnTemplate);
        }
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.EndHorizontal();
        UnityEngine.GUILayout.Label(L("Язык сохраняется в профиле. При следующем запуске язык последнего сохраненного профиля будет выбран автоматически."), _statusStyle);

        Section("Профили");
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(40));
        UnityEngine.GUILayout.Label(L("Название"), _actionLabelStyle, UnityEngine.GUILayout.Width(90), UnityEngine.GUILayout.Height(27));
        _profileName = UnityEngine.GUILayout.TextField(_profileName, _textFieldStyle, UnityEngine.GUILayout.Height(27));
        if (UnityEngine.GUILayout.Button(L("СОХРАНИТЬ"), _buttonStyle, UnityEngine.GUILayout.Width(110), UnityEngine.GUILayout.Height(27))) SaveProfile(_profileName);
        if (UnityEngine.GUILayout.Button(L("ОБНОВИТЬ"), _buttonStyle, UnityEngine.GUILayout.Width(105), UnityEngine.GUILayout.Height(27))) RefreshProfiles();
        UnityEngine.GUILayout.EndHorizontal();
        UnityEngine.GUILayout.Label(L("Профиль хранит все переключатели, множители, значения редакторов, настройки полета и локализацию. Последний сохраненный профиль загружается автоматически при запуске."), _statusStyle);

        if (!_profilesScanned) RefreshProfiles();
        if (_profiles.Count == 0)
        {
            UnityEngine.GUILayout.Label(L("Сохраненных профилей пока нет."), _statusStyle);
        }
        else
        {
            for (int i = 0; i < _profiles.Count; i++)
            {
                string name = _profiles[i];
                UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(36));
                UnityEngine.GUILayout.Label(name, _actionLabelStyle, UnityEngine.GUILayout.Height(25));
                if (UnityEngine.GUILayout.Button(L("ЗАГРУЗИТЬ"), _buttonStyle, UnityEngine.GUILayout.Width(105), UnityEngine.GUILayout.Height(25))) { LoadProfile(name); break; }
                if (UnityEngine.GUILayout.Button(L("УДАЛИТЬ"), _dangerButtonStyle, UnityEngine.GUILayout.Width(90), UnityEngine.GUILayout.Height(25))) { DeleteProfile(name); break; }
                UnityEngine.GUILayout.EndHorizontal();
            }
        }

        Section("Система");
        if (UnityEngine.GUILayout.Button(L("ОТКЛЮЧИТЬ ВСЕ ФУНКЦИИ"), _orangeButtonStyle, UnityEngine.GUILayout.Height(34))) DisableAllFunctions();
        UnityEngine.GUILayout.Label(L("Кнопка отключает все активные переключаемые читы и восстанавливает временно измененные параметры. Уже примененные деньги, уровень, характеристики и выданные предметы не откатываются."), _statusStyle);

        Section("Диагностика");
        UnityEngine.GUILayout.Label(L("Установлено Harmony-патчей: ") + _patchOk + " / 13", _statusStyle);
        UnityEngine.GUILayout.Label("TG.Main: " + (FindType("Awaken.TG.Main.Heroes.Hero") != null ? L("найден") : L("НЕ НАЙДЕН")), _statusStyle);
        UnityEngine.GUILayout.Label("Hero.Current: " + (Hero() != null ? L("найден") : L("не загружен")), _statusStyle);
        UnityEngine.GUILayout.Label((Language == 1 ? "Выбранный предмет: " : "Selected item: ") + (_selectedItem != null ? L("найден") : (Language == 1 ? "нет" : "no")), _statusStyle);
        UnityEngine.GUILayout.Label("Item Spawner: " + (_itemTemplatesLoaded ? ((Language == 1 ? "база " : "database ") + _itemTemplates.Count) : L("не загружен")), _statusStyle);
        UnityEngine.GUILayout.Label((Language == 1 ? "Полет: " : "Flight: ") + (FlightEnabled ? L("включен") : L("выключен")), _statusStyle);
        if (_patchErrors.Count > 0)
        {
            Section("Ошибки патчей");
            for (int i = 0; i < _patchErrors.Count; i++) UnityEngine.GUILayout.Label(L(_patchErrors[i]), _statusStyle);
        }
        else UnityEngine.GUILayout.Label(L("Все патчи установлены без ошибок."), _goodStatusStyle);
        UnityEngine.GUILayout.Space(8);
        if (UnityEngine.GUILayout.Button(L("Повторно синхронизировать значения с персонажем"), _buttonStyle, UnityEngine.GUILayout.Height(30)))
        {
            object h = Hero(); if (h != null) SyncEditorsFromHero(h);
        }
    }

    static void Section(string title)
    {
        UnityEngine.GUILayout.Space(8);
        UnityEngine.GUILayout.Label(L(title).ToUpperInvariant(), _sectionStyle, UnityEngine.GUILayout.Height(28));
        UnityEngine.GUILayout.Space(2);
    }

    static bool Toggle(string label, bool value, string hotkey)
    {
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(40));
        bool nv = value;
        UnityEngine.GUIStyle markerStyle = value ? _toggleMarkerOnStyle : _toggleMarkerOffStyle;
        if (UnityEngine.GUILayout.Button(value ? L("ВКЛ") : L("ВЫКЛ"), markerStyle, UnityEngine.GUILayout.Width(58), UnityEngine.GUILayout.Height(28))) nv = !value;
        float labelWidth = UnityEngine.Mathf.Max(230f, _windowRect.width - 300f);
        if (UnityEngine.GUILayout.Button(L(label), _toggleLabelStyle, UnityEngine.GUILayout.Width(labelWidth), UnityEngine.GUILayout.Height(28))) nv = !value;
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label(hotkey, _hotkeyStyle, UnityEngine.GUILayout.Width(125), UnityEngine.GUILayout.Height(28));
        UnityEngine.GUILayout.EndHorizontal();
        return nv;
    }

    static bool TryParseFlexibleFloat(string text, out float value)
    {
        value = 0f;
        if (text == null) return false;
        string normalized = text.Trim().Replace(',', '.');
        if (normalized.Length == 0 || normalized == "+" || normalized == "-" || normalized == "." || normalized == "+." || normalized == "-.") return false;
        float parsed;
        try { parsed = System.Convert.ToSingle(normalized, System.Globalization.CultureInfo.InvariantCulture); }
        catch { return false; }
        if (float.IsNaN(parsed) || float.IsInfinity(parsed)) return false;
        value = parsed;
        return true;
    }

    static string FormatFloat(float value)
    {
        return value.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
    }

    static NumericInputState GetNumericInputState(string controlName, string initialText)
    {
        NumericInputState state;
        if (!_numericInputStates.TryGetValue(controlName, out state))
        {
            state = new NumericInputState();
            state.Text = initialText;
            _numericInputStates[controlName] = state;
        }
        return state;
    }

    static void NumericFloatField(string key, ref float value, float min, float max, float width, float height)
    {
        string controlName = "FoATrainer.Float." + key;
        NumericInputState state = GetNumericInputState(controlName, FormatFloat(value));
        bool focusedBefore = UnityEngine.GUI.GetNameOfFocusedControl() == controlName;
        if (!state.Initialized || (!focusedBefore && !state.WasFocused && UnityEngine.Mathf.Abs(state.LastFloatValue - value) > 0.0001f))
            state.Text = FormatFloat(value);

        UnityEngine.GUI.SetNextControlName(controlName);
        state.Text = UnityEngine.GUILayout.TextField(state.Text, _textFieldStyle, UnityEngine.GUILayout.Width(width), UnityEngine.GUILayout.Height(height));

        float parsed;
        if (TryParseFlexibleFloat(state.Text, out parsed)) value = Clamp(parsed, min, max);

        bool focusedAfter = UnityEngine.GUI.GetNameOfFocusedControl() == controlName;
        UnityEngine.Event current = UnityEngine.Event.current;
        bool finishWithEnter = focusedAfter && current != null && current.type == UnityEngine.EventType.KeyDown &&
            (current.keyCode == UnityEngine.KeyCode.Return || current.keyCode == UnityEngine.KeyCode.KeypadEnter);
        if ((state.WasFocused && !focusedAfter) || finishWithEnter)
        {
            state.Text = FormatFloat(value);
            if (finishWithEnter)
            {
                UnityEngine.GUI.FocusControl("");
                current.Use();
                focusedAfter = false;
            }
        }
        state.WasFocused = focusedAfter;
        state.LastFloatValue = value;
        state.Initialized = true;
    }

    static void NumericIntField(string key, ref int value, int min, int max, float width, float height)
    {
        string controlName = "FoATrainer.Int." + key;
        NumericInputState state = GetNumericInputState(controlName, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        bool focusedBefore = UnityEngine.GUI.GetNameOfFocusedControl() == controlName;
        if (!state.Initialized || (!focusedBefore && !state.WasFocused && state.LastIntValue != value))
            state.Text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);

        UnityEngine.GUI.SetNextControlName(controlName);
        state.Text = UnityEngine.GUILayout.TextField(state.Text, _textFieldStyle, UnityEngine.GUILayout.Width(width), UnityEngine.GUILayout.Height(height));

        try
        {
            int parsed = System.Convert.ToInt32(state.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            if (parsed < min) parsed = min;
            if (parsed > max) parsed = max;
            value = parsed;
        }
        catch { }

        bool focusedAfter = UnityEngine.GUI.GetNameOfFocusedControl() == controlName;
        UnityEngine.Event current = UnityEngine.Event.current;
        bool finishWithEnter = focusedAfter && current != null && current.type == UnityEngine.EventType.KeyDown &&
            (current.keyCode == UnityEngine.KeyCode.Return || current.keyCode == UnityEngine.KeyCode.KeypadEnter);
        if ((state.WasFocused && !focusedAfter) || finishWithEnter)
        {
            state.Text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (finishWithEnter)
            {
                UnityEngine.GUI.FocusControl("");
                current.Use();
                focusedAfter = false;
            }
        }
        state.WasFocused = focusedAfter;
        state.LastIntValue = value;
        state.Initialized = true;
    }

    static void SyncNumericFloatText(string key, float value)
    {
        string controlName = "FoATrainer.Float." + key;
        NumericInputState state = GetNumericInputState(controlName, FormatFloat(value));
        state.Text = FormatFloat(value);
        state.LastFloatValue = value;
        state.WasFocused = false;
        state.Initialized = true;
    }

    static void SyncNumericIntText(string key, int value)
    {
        string controlName = "FoATrainer.Int." + key;
        NumericInputState state = GetNumericInputState(controlName, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        state.Text = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        state.LastIntValue = value;
        state.WasFocused = false;
        state.Initialized = true;
    }

    static void NumericFloatSlider(string key, ref float value, float sliderMin, float sliderMax, float width)
    {
        bool changedBefore = UnityEngine.GUI.changed;
        UnityEngine.GUI.changed = false;
        float sliderValue = UnityEngine.GUILayout.HorizontalSlider(Clamp(value, sliderMin, sliderMax), sliderMin, sliderMax, UnityEngine.GUILayout.Width(width));
        bool sliderChanged = UnityEngine.GUI.changed;
        UnityEngine.GUI.changed = changedBefore || sliderChanged;
        if (!sliderChanged) return;
        value = Clamp(sliderValue, sliderMin, sliderMax);
        SyncNumericFloatText(key, value);
    }

    static void NumericIntSlider(string key, ref int value, int sliderMin, int sliderMax, float width)
    {
        bool changedBefore = UnityEngine.GUI.changed;
        UnityEngine.GUI.changed = false;
        float sliderValue = UnityEngine.GUILayout.HorizontalSlider(UnityEngine.Mathf.Clamp(value, sliderMin, sliderMax), sliderMin, sliderMax, UnityEngine.GUILayout.Width(width));
        bool sliderChanged = UnityEngine.GUI.changed;
        UnityEngine.GUI.changed = changedBefore || sliderChanged;
        if (!sliderChanged) return;
        value = UnityEngine.Mathf.RoundToInt(sliderValue);
        if (value < sliderMin) value = sliderMin;
        if (value > sliderMax) value = sliderMax;
        SyncNumericIntText(key, value);
    }

    static bool ToggleFloat(string label, bool enabled, ref float value, string hotkey, float min, float max, float sliderMin, float sliderMax)
    {
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(40));
        bool nv = enabled;
        UnityEngine.GUIStyle markerStyle = enabled ? _toggleMarkerOnStyle : _toggleMarkerOffStyle;
        if (UnityEngine.GUILayout.Button(enabled ? L("ВКЛ") : L("ВЫКЛ"), markerStyle, UnityEngine.GUILayout.Width(58), UnityEngine.GUILayout.Height(28))) nv = !enabled;
        float labelWidth = UnityEngine.Mathf.Max(165f, _windowRect.width - 650f);
        if (UnityEngine.GUILayout.Button(L(label), _toggleLabelStyle, UnityEngine.GUILayout.Width(labelWidth), UnityEngine.GUILayout.Height(28))) nv = !enabled;
        NumericFloatField("ToggleFloat." + label, ref value, min, max, 96f, 28f);
        NumericFloatSlider("ToggleFloat." + label, ref value, sliderMin, sliderMax, 165f);
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label(hotkey, _hotkeyStyle, UnityEngine.GUILayout.Width(105), UnityEngine.GUILayout.Height(28));
        UnityEngine.GUILayout.EndHorizontal();
        return nv;
    }

    delegate void SimpleAction();

    static void ActionInt(string label, ref int value, string hotkey, SimpleAction action, int min, int max)
    {
        UnityEngine.GUILayout.BeginHorizontal(_rowStyle, UnityEngine.GUILayout.Height(36));
        UnityEngine.GUILayout.Label("◆", _actionMarkerStyle, UnityEngine.GUILayout.Width(24), UnityEngine.GUILayout.Height(25));
        float labelWidth = UnityEngine.Mathf.Max(196f, _windowRect.width - 479f);
        UnityEngine.GUILayout.Label(L(label), _actionLabelStyle, UnityEngine.GUILayout.Width(labelWidth), UnityEngine.GUILayout.Height(25));
        NumericIntField("ActionInt." + label, ref value, min, max, 120f, 25f);
        if (UnityEngine.GUILayout.Button(L("ПРИМЕНИТЬ"), _buttonStyle, UnityEngine.GUILayout.Width(105), UnityEngine.GUILayout.Height(25))) action();
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label(hotkey, _hotkeyStyle, UnityEngine.GUILayout.Width(125), UnityEngine.GUILayout.Height(25));
        UnityEngine.GUILayout.EndHorizontal();
    }

}
