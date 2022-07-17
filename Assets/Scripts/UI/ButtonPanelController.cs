using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    [SerializeField]
    TextAsset destinationData;
    [SerializeField]
    TextAsset distanceData;
    [SerializeField]
    Destination destinationPrefab;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    GameController GC;
    [SerializeField]
    GameObject resetBtn;
    private ObjectPool<Destination> pool;
    private List<Destination> Destinations = new List<Destination>();
    bool isGenetic = GameController.IsGenetic;
    // Start is called before the first frame update
    void Start()
    {
        UpdateForGenetic();
        UpdateForACO();
        pool = new ObjectPool<Destination>(
            () =>
            {
                /*create function: 
                 * called when there're no objects available in Pool
                 * -> need to create one
                 */
                return Instantiate(destinationPrefab);
            },
            dest =>
            {
                /*onGet function: 
                 * when we ask for 1 object, and there is 1 available in Pool
                 * -> returns an object
                 */
                //active it
                dest.gameObject.SetActive(true);
            },
            dest =>
            {
                /*onRelease function:
                 * when we're done with the object -> give it back to the Pool
                 */
                //deactive it
                dest.gameObject.SetActive(false);
            },
            dest =>
            {
                /*destroy action:
                 * this pool will always spawn objects when we ask for them, even
                 * if it's above the Maximum. Once we spawn it, and it goes to to
                 * return to the pool, if the pool is already filled
                 * => it will destroy the object instead
                 */
                Destroy(dest.gameObject);
            },
            //not important 
            false,
            //default capacity
            50,
            //max size
            100
        );
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GeneratePoints()
    {
        if (!GC.isStarted)
        {
            foreach (Destination dest in Destinations)
            {
                pool.Release(dest);
            }
            Destinations.Clear();
            GameController.TripNumber = int.Parse(panel.gameObject.transform.Find("TripNumberNumber").gameObject.GetComponent<TextMeshProUGUI>().text);

            TextMeshProUGUI text = panel.gameObject.transform.Find("ProblemSizeNumber").gameObject.GetComponent<TextMeshProUGUI>();
            GameController.ProblemSize = int.Parse(text.text);

            List<int> list = Enumerable.Range(0, 100).ToList();
            Extensions.Shuffle(list);

            List<int> idList = list.GetRange(0, GameController.ProblemSize);
            GameController.RealData = new Data(GameController.FullData, idList, GameController.TripNumber);

            for (int i = 0; i < GameController.ProblemSize; i++)
            {
                Destination dest = pool.Get();
                Destinations.Add(dest);
                dest.transform.position = new Vector3(GameController.RealData.POI[i].Location.x, GameController.RealData.POI[i].Location.y);
                //Instantiate(destinationPrefab, GameController.RealData.POI[i].Location, Quaternion.identity);
            }
            GC.isGenerated = true;
        }
    }

    public void ChangeStatus()
    {
        isGenetic = !isGenetic;
        UpdateForACO();
        UpdateForGenetic();
        GameController.IsGenetic = isGenetic;

    }
    public void UpdateForGenetic()
    {

        panel.gameObject.transform.Find("MutationRateTxt").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("MutationRateNumber").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("MutationRateSlider").gameObject.SetActive(isGenetic);

        panel.gameObject.transform.Find("ElitismTxt").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("ElitismNumber").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("ElitismSlider").gameObject.SetActive(isGenetic);

        panel.gameObject.transform.Find("CrossoverRateTxt").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("CrossoverRateNumber").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("CrossoverRateSlider").gameObject.SetActive(isGenetic);

        panel.gameObject.transform.Find("PoolSizeTxt").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("PoolSizeNumber").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("PoolSizeSlider").gameObject.SetActive(isGenetic);

        

    }
    public void UpdateForACO()
    {
        ///Set sliders for aco
        panel.gameObject.transform.Find("AlphaTxt").gameObject.gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("AlphaNumber").gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("AlphaSlider").gameObject.SetActive(!isGenetic);

        panel.gameObject.transform.Find("BetaTxt").gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("BetaNumber").gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("BetaSlider").gameObject.SetActive(!isGenetic);

        panel.gameObject.transform.Find("NumberOfAntsTxt").gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("NumberOfAntsNumber").gameObject.SetActive(!isGenetic);
        panel.gameObject.transform.Find("NumberOfAntsSlider").gameObject.SetActive(!isGenetic);
    }
    public void StartBtn()
    {
        if (GC.isGenerated)
        {
            GameController.PoolSize = int.Parse(panel.gameObject.transform.Find("PoolSizeNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            GameController.ElitismRate = float.Parse(panel.gameObject.transform.Find("ElitismNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            GameController.MutationRate = float.Parse(panel.gameObject.transform.Find("MutationRateNumber").gameObject.GetComponent<TextMeshProUGUI>().text);
            GameController.CrossoverRate = float.Parse(panel.gameObject.transform.Find("CrossoverRateNumber").gameObject.GetComponent<TextMeshProUGUI>().text);

            GameController.manager.SetStrategy(new GeneticAlgorithm(GameController.RealData, GameController.PoolSize,
                GameController.ElitismRate, GameController.CrossoverRate, GameController.MutationRate));

            GC.isStarted = !GC.isStarted;
            //Time.timeScale = 1f;
            panel.gameObject.transform.Find("StartBtn").gameObject.SetActive(false);
            resetBtn.SetActive(true);
        }
    }
    public void ResetBtn()
    {
        panel.gameObject.transform.Find("StartBtn").gameObject.SetActive(true);
        resetBtn.SetActive(false);
        GC.drawingPath = false;
        GC.isStarted = false;
        foreach (var item in GC.currentLines)
        {
            Destroy(item);
        }
        GC.currentLines.Clear();
        foreach (var item in GC.currentArrows)
        {
            Destroy(item);
        }
        GC.currentArrows.Clear();
        GC.linesToBeActive.Clear();
        GC.arrowsToBeActive.Clear();

        GC.Awake();
    }
    public void StopBtn()
    {
        if (Time.timeScale == 1f)
        {
            panel.gameObject.transform.Find("StopBtn").gameObject.transform.Find("StopBtnTxt").GetComponent<TextMeshProUGUI>().text = "Resume";
            Time.timeScale = 0;
        }
        else
        {
            panel.gameObject.transform.Find("StopBtn").gameObject.transform.Find("StopBtnTxt").GetComponent<TextMeshProUGUI>().text = "Stop";
            Time.timeScale = 1;
        }
    }
}
