using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour {
    public void backToMainMenu() {
        Time.timeScale = 1;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void saveGame() {
        DataPersistenceManager.instance.SaveGame();
        GameObject.FindWithTag("CanvasManager").gameObject.transform.Find("SettingsScreen")
            .gameObject.GetComponent<SettingsScreenManager>().showSettingsScreen("OpenSettings");
    }

    public void loadGame() {
        DataPersistenceManager.instance.setFromMainMenu(true);
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.getSceneToLoadForMainMenu());
        Time.timeScale = 1;
    }
}
