using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;   //키넥트 센서
    private BodyFrameReader _Reader;//뼈마디 데이터를 읽는 클래스
    private Body[] _Data = null;    //뼈마디 데이터
    
    
    public Body[] GetData()
    {
        return _Data;
    }
    

    void Start () 
    {
        // 키넥트 센서 작동
        _Sensor = KinectSensor.GetDefault();

        // 키넥트 센서가 있을 경우
        if (_Sensor != null)
        {
            // 바디프레임리더에 값을 전달
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }   
    }
    
    void Update () 
    {
        // 키넥트 센서가 있을 경우 계속해서 데이터를 받아온다.
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                // 데이터가 없을 경우 인식되는 바디 데이터를 가져온다.
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                // 뼈데이터 업데이트
                frame.GetAndRefreshBodyData(_Data);
                
                frame.Dispose();
                frame = null;
            }
        }    
    }
    
    //어플리케이션 종료시 
    void OnApplicationQuit()
    {
        // 바디프레임리더가 있을 경우 
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
        
        // 키넥트 센서가 켜져있을 경우 종료
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            
            _Sensor = null;
        }
    }
}
