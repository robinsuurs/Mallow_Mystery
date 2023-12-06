using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dialogue.RunTime;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

//Youtube video used: https://www.youtube.com/watch?v=aUi9aijvpgs&t=538s

public class DataPersistenceManager : MonoBehaviour {
    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    [SerializeField] private bool startFresh;
    [SerializeField] private bool encryptData;
    [SerializeField] private GameEventStandardAdd showItems;
    [SerializeField] private GameEventStandardAdd endSceneLoaded;
    [SerializeField] private LevelManager _levelManager;
    
    private GameData _gameData;
    private List<IDataPersistence> dataPersistences;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one DataPersistenceManager found, Shit hits the fan! Or Destroying the new one");
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        _levelManager.sceneSwitchData = null;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        this.dataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            Camera.main.gameObject.GetComponent<Follow_Player>().setFollowPlayer(); //TODO BM: change this, this is not how it is supposed to work
            Camera.main.gameObject.GetComponent<SeeThrough>().setFollowPlayer();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceMon = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IDataPersistence>();
        IEnumerable<IDataPersistence> dataPersistenceScript = Resources.FindObjectsOfTypeAll<ScriptableObject>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceMon.Concat(dataPersistenceScript));
    }

    private void OnApplicationQuit() {
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            SaveGame();
        }
    }

    public void NewGame() {
        this._gameData = new GameData("");
    }

    private void LoadGame() {
        if (this._gameData == null) {
            this._gameData = dataHandler.Load();
        }
        if (this._gameData == null || startFresh) {
            Debug.Log("No GameData found. A new Game needs to be created");
            NewGame();
        }
        else {
            foreach (IDataPersistence dataPersistenceObj in dataPersistences) {
                dataPersistenceObj.LoadData(_gameData);
            }

            LoadDialogueStates();
            showItems.Raise();
        }

        if (!SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            // GameObject.FindWithTag("CanvasManager").transform.Find("ShortcutImages").gameObject.SetActive(true);
            _levelManager.SpawnPlayer(_gameData);
            endSceneLoaded.Raise();
        }
    }

    public void setFromMainMenu () {
        this._levelManager.sceneSwitchData = null;
    }

    public string getSceneToLoadForMainMenu() {
        return _gameData.sceneName;
    }

    public void SaveGame () {
        if (this._gameData == null) {
            Debug.Log("No GameData found. A new Game needs to be created before being saved");
            return;
        }
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistences) {
            dataPersistenceObj.SaveData(ref _gameData);
        }
        
        SaveDialogueStates();

        _gameData.sceneName = SceneManager.GetActiveScene().name;
        _gameData.playerLocation = GameObject.FindWithTag("Player").transform.position;
        
        dataHandler.Save(_gameData);
    }

    public bool hasGameData() {
        return _gameData != null;
    }

    private void SaveDialogueStates() {
        List<DialogueContainer> dialogueContainers = Resources.LoadAll<DialogueContainer>("").ToList();
        _gameData.alreadyHadConversations.Clear();

        foreach (var dialogueContainer in dialogueContainers.Where(dialogueContainer => dialogueContainer.alreadyHadConversation)) {
            _gameData.alreadyHadConversations.Add(dialogueContainer.name);
        }
    }
    
    private void LoadDialogueStates() {
        List<DialogueContainer> dialogueContainers = Resources.LoadAll<DialogueContainer>("").ToList();
        foreach (var dialogueContainer in from dialogueContainer in dialogueContainers from name in _gameData.alreadyHadConversations where dialogueContainer.name.Equals(name) select dialogueContainer) {
            dialogueContainer.alreadyHadConversation = true;
        }
    }

    public void resetToStandardValues() {
        _levelManager.sceneSwitchData = null;
    }

    public GameData getGameData() {
        return _gameData;
    }
}
