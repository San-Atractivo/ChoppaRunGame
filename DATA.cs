using UnityEngine;
using System.Collections;

public class DATA {

    public const int MAXENERGY = 7;

    static DATA SingleTon;

    public static DATA getData()
    {
        if (SingleTon == null) SingleTon = new DATA();
        return SingleTon;
    }

	// 유저 정보
	public int MONEY
    {
		get { 
			if (!PlayerPrefs.HasKey ("MONEY"))	PlayerPrefs.SetInt ("MONEY", 100);
			return PlayerPrefs.GetInt ("MONEY");}
		set { PlayerPrefs.SetInt ("MONEY", value); }
	}

	public int ENERGY
    {
		get
        { 
			if (!PlayerPrefs.HasKey ("ENERGY"))	PlayerPrefs.SetInt ("ENERGY", MAXENERGY);
			return PlayerPrefs.GetInt ("ENERGY");
        }
		set { PlayerPrefs.SetInt ("ENERGY", value); }
	}

    public int BESTSCORE
    {
        get
        {
            if (!PlayerPrefs.HasKey("BESTSCORE")) PlayerPrefs.SetInt("BESTSCORE", 0);
            return PlayerPrefs.GetInt("BESTSCORE");
        }
        set { PlayerPrefs.SetInt("BESTSCORE", value); }
    }

    public int MAXDISTANCE
    {
        get
        {
            if (!PlayerPrefs.HasKey("MAXDISTANCE")) PlayerPrefs.SetInt("MAXDISTANCE", 0);
            return PlayerPrefs.GetInt("MAXDISTANCE");
        }
        set { PlayerPrefs.SetInt("MAXDISTANCE", value); }
    }



	// 캐릭터 스텟
    public int ALLSTAT
    {
        get { return FASTSTAT + SHILDESTAT + MAGNETSTAT; }
    }

    public int FASTLEVEL
    {
        get
        {
            if (!PlayerPrefs.HasKey("FASTLEVEL")) PlayerPrefs.SetInt("FASTLEVEL", 1);
            return PlayerPrefs.GetInt("FASTLEVEL");
        }
        set { PlayerPrefs.SetInt("FASTLEVEL", value); }
    }

	public int FASTSTAT
    {
		get
        {
            if (!PlayerPrefs.HasKey("FASTSTAT")) PlayerPrefs.SetInt("FASTSTAT", 1);
            return PlayerPrefs.GetInt ("FASTSTAT");
        }
		set { PlayerPrefs.SetInt ("FASTSTAT", value);}
	}

    public int SHILDELEVEL
    {
        get
        {
            if (!PlayerPrefs.HasKey("SHILDELEVEL")) PlayerPrefs.SetInt("SHILDELEVEL", 1);
            return PlayerPrefs.GetInt("SHILDELEVEL");
        }
        set { PlayerPrefs.SetInt("SHILDELEVEL", value); }
    }

    public int SHILDESTAT
    {
		get
        {
            if (!PlayerPrefs.HasKey("SHILDESTAT")) PlayerPrefs.SetInt("SHILDESTAT", 1);
            return PlayerPrefs.GetInt ("SHILDESTAT");
        }
		set { PlayerPrefs.SetInt ("SHILDESTAT", value); }
	}

    public int MAGNETLEVEL
    {
        get
        {
            if (!PlayerPrefs.HasKey("MAGNETLEVEL")) PlayerPrefs.SetInt("MAGNETLEVEL", 1);
            return PlayerPrefs.GetInt("MAGNETLEVEL");
        }
        set { PlayerPrefs.SetInt("MAGNETLEVEL", value); }
    }

    public int MAGNETSTAT
    {
		get
        {
            if (!PlayerPrefs.HasKey("MAGNETSTAT")) PlayerPrefs.SetInt("MAGNETSTAT", 1);
            return PlayerPrefs.GetInt ("MAGNETSTAT");
        }
		set { PlayerPrefs.SetInt("MAGNETSTAT", value); }
	}

    //게임 설정
    public string ENERGYTIME
    {
        get
        {
            if (!PlayerPrefs.HasKey("ENERGYTIME")) PlayerPrefs.SetString("ENERGYTIME", null);
            return PlayerPrefs.GetString("ENERGYTIME");
        }
        set { PlayerPrefs.SetString("ENERGYTIME", value); }
    }

    public bool FPSView
    {
        get
        {
            if (!PlayerPrefs.HasKey("FPSVIEW")) PlayerPrefs.SetString("FPSVIEW", "0");
            return PlayerPrefs.GetString("FPSVIEW").Equals("1") ? true : false;
        }
        set { PlayerPrefs.SetString("FPSVIEW", value ? "1" : "0" ); }

    }
}
