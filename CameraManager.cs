using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Greyman;

public class CameraManager : MonoBehaviour {

	private static Rect Single = new Rect (0f, 0f, 1f, 1f);
	private static Rect TopHalf = new Rect (0f, .5f, 1f, .5f);
	private static Rect BottomHalf = new Rect (0f, 0f, 1f, .5f);
	private static Rect RightHalf = new Rect (.5f, 0f, .5f, 1f);
	private static Rect LeftHalf = new Rect (0f, 0f, .5f, 1f);
	private static Rect TopLeft = new Rect (0f, .5f, .5f, .5f);
	private static Rect TopRight = new Rect (.5f, .5f, .5f, .5f);
	private static Rect BottomLeft = new Rect (0f, 0f, .5f, .5f);
	private static Rect BottomRight = new Rect (.5f, 0f, .5f, .5f);

	public static CameraManager instance;

	public CamMode mode;

    public List<MultiObjectFollow> ActiveCameras = new List<MultiObjectFollow>();
    public MultiObjectFollow p1Cam;
    public MultiObjectFollow p2Cam;
    public MultiObjectFollow p3Cam;
    public MultiObjectFollow p4Cam;

    public float rot = 60f;
    public float fov;

	public Transform CameraHolder;

	public float currentfps;

	public enum CamMode {
		Horizontal,
        Vertical
	}

	void Start () {
		//set static instance
        if (instance == null)
		    instance = this;

	}

	void Update () {
		fps ();

	}

	void fps() {
		float t = Time.deltaTime;
		currentfps = 1 / t;
	}
		

	public bool IsObjectTracked(GameObject obj) {
		bool result = false;
		foreach (MultiObjectFollow mofCam in ActiveCameras) {
			if (mofCam.trackers.Contains (obj))
				result = true;
		}
		return result;
	}

    public int CameraCount()
    {
        int a = 0;
        if (p1Cam.gameObject.activeInHierarchy)
            a++;
        if (p2Cam.gameObject.activeInHierarchy)
            a++;
        if (p3Cam.gameObject.activeInHierarchy)
            a++;
        if (p4Cam.gameObject.activeInHierarchy)
            a++;
        if (a == ActiveCameras.Count)
            return a;
        Debug.LogError("Check your active cameras!");
        return - 1;
    }

    public MultiObjectFollow AddCamera(GameObject tracker)
    {
        int count = CameraCount();
        //get next unused camera
        MultiObjectFollow mof;
        if (count == 0)
            mof = p1Cam;
        else if (count == 1)
            mof = p2Cam;
        else if (count == 2)
            mof = p3Cam;
        else
            mof = p3Cam;

        mof.gameObject.SetActive(true);

        mof.trackers.Clear();
        mof.trackers.Add(tracker);

        ActiveCameras.Add(mof);

        setIndicators();
        AdjustCameraSizes();
        StartCoroutine(displayCameraDammit(mof.uiCamera.UICam));

        return mof;
    }
    void setIndicators()
    {
        foreach(MultiObjectFollow mof in ActiveCameras)
        {
            
            //TODO add all other players for OffScreen Indicator
            foreach (ActivePlayer player in GameManager.instance.players)
            {
                //mof.uiCamera.osi.AddIndicator(player.unit, 0);
            }
        }
    }

    /*
	// Split Cameras
	public MultiObjectFollow CreateNewTrackerCamera(GameObject tracker) {
		GameObject cam = new GameObject ();
		if (CameraHolder != null)
			cam.transform.SetParent (CameraHolder);
		cam.name = "Dynamic SS Split Camera";
		cam.transform.position = (ActiveCameras.Count == 0) ? new Vector3 (0f, 20f, 0f) : ActiveCameras[0].gameObject.transform.position;
		cam.transform.Rotate (new Vector3 (rot, 0, 0));
		Camera c = cam.AddComponent<Camera> ();
        c.fieldOfView = fov;
		if (!anyAudioListeners())
			cam.AddComponent<AudioListener> ();
		c.cullingMask &= ~(1 << LayerMask.NameToLayer ("P1UI")) & ~(1 << LayerMask.NameToLayer ("P2UI")) 
			& ~(1 << LayerMask.NameToLayer ("P3UI")) & ~(1 << LayerMask.NameToLayer ("P4UI")) & ~(1 << LayerMask.NameToLayer ("UI"));

		MultiObjectFollow mof = cam.AddComponent<MultiObjectFollow> ();
		mof.trackers.Add (tracker);
        //Antialiasing aa = cam.AddComponent<Antialiasing> ();
        //BloomOptimized bloom = cam.AddComponent<BloomOptimized> ();
        //bloom.threshold = .5f;
        //bloom.intensity = .5f;
        //SSAOPro ssao = cam.AddComponent<SSAOPro>();
        //ssao.Samples = SSAOPro.SampleCount.VeryLow;

		ActiveCameras.Add (mof);

		AdjustCameraSizes ();

		return mof;
	}

	public void MakeUICamera(MultiObjectFollow target, int player) {
		if (player == 0)
			player++;
		
		GameObject camObj = new GameObject();
		camObj.name = "UICam";
		camObj.transform.SetParent (target.gameObject.transform);

		Camera UICam = camObj.AddComponent<Camera> ();
        //set flags
        UICam.fieldOfView = fov;
		UICam.clearFlags = CameraClearFlags.Depth;
		UICam.cullingMask = (1 << LayerMask.NameToLayer ("UI")) | (1 << LayerMask.NameToLayer ("P" +  player + "UI"));

		GameObject canvasObj = new GameObject ();
		canvasObj.transform.SetParent (camObj.transform);
		canvasObj.name = "Canvas P" + player;
		canvasObj.layer = LayerMask.NameToLayer ("P" +  player +"UI");
		Canvas canvas = canvasObj.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.planeDistance = 5;
		canvas.worldCamera = UICam;

		UICamera uic = new UICamera ();
		uic.UICam = UICam;
		uic.canvas = canvas;

		target.uiCamera = uic;

		camObj.transform.SetParent (target.gameObject.transform);
		camObj.transform.localPosition = Vector3.zero;
		camObj.transform.localRotation = Quaternion.identity;

		AdjustCameraSizes ();

        UICam.allowMSAA = false;
        UICam.allowHDR = false;
        //UICam.allowMSAA = true;
        //UICam.allowHDR = true;
        StartCoroutine(displayCameraDammit(UICam));
    }
    */

    public IEnumerator displayCameraDammit(Camera cam)
    {
        for(int i = 0; i < 60; i++)
        {
            cam.enabled = !cam.enabled;
            cam.enabled = !cam.enabled;
            yield return null;
        }
        cam.enabled = true;
    }

	void AdjustCameraSizes() {
		if (CameraCount() == 1) {
            SetCameras(p1Cam, Single);
        } else if (CameraCount() == 2) {
			if (mode == CamMode.Horizontal)
            {
                SetCameras(p1Cam, TopHalf);
                SetCameras(p2Cam, BottomHalf);
            } else
            {
                SetCameras(p1Cam, LeftHalf);
                SetCameras(p2Cam, RightHalf);
            }
		}
        else if (CameraCount() == 3)
        {
            if (mode == CamMode.Horizontal)
            {
                SetCameras(p1Cam, TopHalf);
                SetCameras(p2Cam, BottomLeft);
                SetCameras(p3Cam, BottomRight);
            }
            else
            {
                SetCameras(p1Cam, LeftHalf);
                SetCameras(p2Cam, TopRight);
                SetCameras(p3Cam, BottomRight);
            }
        }
        else if (CameraCount() == 4)
        {
            SetCameras(p1Cam, TopLeft);
            SetCameras(p2Cam, TopRight);
            SetCameras(p3Cam, BottomLeft);
            SetCameras(p4Cam, BottomRight);
        }
        //maybe not necessary
        for (int i = 0; i < ActiveCameras.Count; i++)
        {
            MultiObjectFollow cam = ActiveCameras[i];
            if (cam.uiCamera != null)
              StartCoroutine(displayCameraDammit(cam.uiCamera.UICam));
        }

    }
    public void SetCameras(MultiObjectFollow mof, Rect rect)
    {
        mof.m_Camera.rect = rect;
        mof.uiCamera.UICam.rect = mof.m_Camera.rect;
        //if (mof.uiCamera.osimc == null)
        //    mof.uiCamera.osimc = mof.uiCamera.osi.gameObject.GetComponent<OffScreenIndicatorManagerCanvas>();
        // FIXME mof.uiCamera.osimc.UpdateScreen();
    }

	public void DeleteCamera(MultiObjectFollow cam) {
		ActiveCameras.Remove (cam);
		Destroy (cam.gameObject);

		AdjustCameraSizes ();
	}

	bool anyAudioListeners() {
		bool res = false;
		foreach (MultiObjectFollow c in ActiveCameras) {
			if (c.gameObject.GetComponent<AudioListener> () != null)
				res = true;
		}
		return res;
	}
}
