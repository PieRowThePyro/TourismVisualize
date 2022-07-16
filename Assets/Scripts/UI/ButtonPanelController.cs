using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

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
    private ObjectPool<Destination> pool;
    private List<Destination> Destinations = new List<Destination>();
    bool isGenetic = true;
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
            bullet =>
            {
                /*onGet function: 
                 * when we ask for 1 object, and there is 1 available in Pool
                 * -> returns an object
                 */
                //active it
                bullet.gameObject.SetActive(true);
            },
            bullet =>
            {
                /*onRelease function:
                 * when we're done with the object -> give it back to the Pool
                 */
                //deactive it
                bullet.gameObject.SetActive(false);
            },
            bullet =>
            {
                /*destroy action:
                 * this pool will always spawn objects when we ask for them, even
                 * if it's above the Maximum. Once we spawn it, and it goes to to
                 * return to the pool, if the pool is already filled
                 * => it will destroy the object instead
                 */
                Destroy(bullet.gameObject);
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

        foreach (Destination dest in Destinations)
        {
            pool.Release(dest);
        }
        Destinations.Clear();
        TextMeshProUGUI text = panel.gameObject.transform.Find("ProblemSizeNumber").gameObject.GetComponent<TextMeshProUGUI>();
        GameController.ProblemSize = int.Parse(text.text);

        List<int> list = Enumerable.Range(0, 100).ToList();
        Extensions.Shuffle(list);

        List<int> idList = list.GetRange(0, GameController.ProblemSize);
        GameController.RealData = new Data(GameController.FullData, idList);

        for (int i = 0; i < GameController.ProblemSize; i++)
        {
            Destination dest = pool.Get();
            Destinations.Add(dest);
            dest.transform.position = new Vector3(GameController.RealData.POI[i].Location.x, GameController.RealData.POI[i].Location.y);
            //Instantiate(destinationPrefab, GameController.RealData.POI[i].Location, Quaternion.identity);
        }


    }

    public void ChangeStatus()
    {
        isGenetic = !isGenetic;
        UpdateForACO();
        UpdateForGenetic();

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

        panel.gameObject.transform.Find("ProblemSizeTxt").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("ProblemSizeNumber").gameObject.SetActive(isGenetic);
        panel.gameObject.transform.Find("ProblemSizeSlider").gameObject.SetActive(isGenetic);

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

}
