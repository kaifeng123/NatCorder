
using NatSuite.Examples;
using UnityEngine;
using UnityEngine.UI;
public class UiCanvas : MonoBehaviour
{
    private Text uiText;
    private Text btnText;
    public ReplayCam cam;
    private int timeNum;
    private RecordType recordType;
    void Awake()
    {
        uiText = transform.Find("Text").GetComponent<Text>();
        btnText = transform.Find("Button/Text").GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(Btn);
       // cam.replayAction += replayAction;
    }
    private void OnEnable()
    {
        recordType = RecordType.Null; 
        SetRecordType();
    }
    private void Btn()
    {
        SetRecordType();
    }
    void SetRecordType()
    {
        switch (recordType)
        {
            case RecordType.Null:
                recordType = RecordType.StartRecord;
                uiText.text = "";
                break;
            case RecordType.StartRecord:
                StartRectord();
                recordType = RecordType.StopRecord;
                break;
            case RecordType.StopRecord:
                StopRectord();
                recordType = RecordType.StartRecord;
                break;
            default:
                break;
        }
        btnText.text = recordType == RecordType.StopRecord ? "停止录屏" : "开始录屏";
    }
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 开始录屏,调用ReplayCam.StartRecording()方法
    /// </summary>
    public void StartRectord()
    {
        timeNum = 0;
        cam.StartRecording();
        //定时的方法
        TimerManager.instance.DoLoop(1000, SetTimeText);
    }
    /// <summary>
    /// 结束录屏,调用ReplayCam.StopRectord()方法
    /// </summary>
    public void StopRectord()
    {
        cam.StopRecording();
        TimerManager.instance.RemoveHandler(SetTimeText);
      //  uiText.text = "正在转译编码,缓存视屏";
        uiText.text = "视屏保存完成,时长:" + timeNum.ToString() + "秒";
    }
    /// <summary>
    /// 设置当前录屏时长
    /// </summary>
    public void SetTimeText()
    {
        timeNum++;
        uiText.text = "录屏时长:" + timeNum.ToString() + "秒";
    }
    /// <summary>
    /// sdk录屏,保存完成后调用这个方法
    /// </summary>
    private void replayAction()
    {
        uiText.text = "视屏保存完成,时长:" + timeNum.ToString() + "秒";
    }
}
public enum RecordType
{
    Null,
    StartRecord,
    StopRecord
}