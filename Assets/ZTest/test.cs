using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Buffs;
using UnityEngine.SceneManagement;
public interface It1 {
    void Reset();
}
public interface It2 {
    void Reset();
}

public class Player:It1,It2{
    public TimerManager timerManager;
    public void Reset() {
       // Debug.Log("reset");
    }
}
public class test : MonoBehaviour {
    public ExtraScrollView scroll;
    public ExtraScrollItem item;
    BuffManager bmanager ;
    TimerManager tmanager = new TimerManager();
    BridgeData Data;
	// Use this for initialization
	void Start () {
        Player p = new Player();
        p.timerManager = tmanager;
        Data = new BridgeData(p);
        bmanager = new BuffManager(Data);

        var b = new Protection(bmanager, 10);
        b.AddEffect();
        b = new Protection(bmanager, 5);
        b.AddEffect();

        It1 t1 = p as It1;
        p.Reset();
        It2 t2 = p as It2;
        p.Reset();

        AtlasManager.Cache(new string[]{"test","dd"});
      
        InvokeRepeating("t",0,1);
       // Invoke("tt",5);

	}
    void Update() {
        tmanager.TimersTick(Time.deltaTime);
       // print(bmanager.collection.Count + " " +tmanager.timerCount);
    }
    void t() {
        var s = AtlasManager.GetSprite("test", "G4");
        Resources.UnloadUnusedAssets();
        Debug.Log(s==null);
    }
    void tt() {
        SceneManager.LoadScene(0);
    }

}
