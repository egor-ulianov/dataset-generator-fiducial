using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{

    public string LocationOfImagesQR;
    public string LocationOfImagesArUco;
    public string LocationOfBackgrounds;
    public Sprite CurrentSprite;
    public Image OwnerImage;
    public Image Background;
    public Image DarkPanel;
    public GameObject Owner;
    public int SizeOfSet;

    public Camera Camera;

    public GameObject FirstCorner;
    public GameObject SecondCorner;
    public GameObject ThirdCorner;
    public GameObject FourthCorner;

    public GameObject Rectangle;

    private int counter;
    private Sprite[] _Sprites;
    private Sprite[] _SpritesArUco;
    private Sprite[] _Backgrounds;

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        this.counter = 0;
        this.loadImages();
        this.loadBackground();
        this.setStartImage(this._Sprites[0]);
    }

    // Update is called once per frame
    void Update()
    {
        this.upadteImage();
    }

    #endregion Unity

    #region Private methods

    private void loadImages()
    {
        if (this.LocationOfImagesQR != "")
            this._Sprites = Resources.LoadAll<Sprite>(this.LocationOfImagesQR);
        if (this.LocationOfImagesQR != "")
            this._SpritesArUco = Resources.LoadAll<Sprite>(this.LocationOfImagesArUco);
    }

    private void loadBackground()
    {
        if (this.LocationOfBackgrounds != "")
            this._Backgrounds = Resources.LoadAll<Sprite>(this.LocationOfBackgrounds);
    }

    private void setStartImage(Sprite sprite)
    {
        if (this._Sprites != null)
        {
            if (this.OwnerImage != null)
                this.OwnerImage.sprite = sprite;
        }
    }

    private void setBackground(Sprite sprite)
    {
        if (this._Backgrounds != null)
        {
            if (this.Background != null)
                this.Background.sprite = sprite;
        }
    }

    private void makeScreenShotsForCode()
    {
        this.Owner.transform.Rotate(0, 0, 0);
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/" + counter.ToString() + "-000-000-000.png");
        this.Owner.transform.Rotate(0, 50, 50);
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/" + counter.ToString() + "-000-050-050.png");
        this.Owner.transform.Rotate(0, -50, -50);
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/" + counter.ToString() + "-000-310-310.png");
    }

    private void upadteImage()
    {
        if (this.SizeOfSet <= this.counter)
            return;

        int currentXRotate = Random.Range(-50, 50);
        int currentYRotate = Random.Range(-50, 50);
        int currentZRotate = Random.Range(-50, 50);

        int currentXPosition = Random.Range(-200, 200);
        int currentYPosition = Random.Range(-130, 130);

        float sizeScale = Random.Range(0.3f, 1.5f);

        if (this.counter % 2 == 0)
            this.setStartImage(this._Sprites[Random.Range(0, this._Sprites.Length)]);
        else
            this.setStartImage(this._SpritesArUco[Random.Range(0, this._SpritesArUco.Length)]);

        this.setBackground(this._Backgrounds[Random.Range(0, this._Backgrounds.Length)]);

        this.Owner.transform.eulerAngles = new Vector3(currentXRotate, currentYRotate, currentZRotate);
        this.Owner.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        this.Owner.transform.localPosition = new Vector3(currentXPosition, currentYPosition, 0);

        float[] t_xs = new float[4];
        t_xs[0] = this.FirstCorner.transform.position.x;
        t_xs[1] = this.SecondCorner.transform.position.x;
        t_xs[2] = this.ThirdCorner.transform.position.x;
        t_xs[3] = this.FourthCorner.transform.position.x;

        float[] t_ys = new float[4];
        t_ys[0] = this.FirstCorner.transform.position.y;
        t_ys[1] = this.SecondCorner.transform.position.y;
        t_ys[2] = this.ThirdCorner.transform.position.y;
        t_ys[3] = this.FourthCorner.transform.position.y;

        this.Rectangle.transform.position = new Vector3(Mathf.Min(t_xs) + (Mathf.Max(t_xs) - Mathf.Min(t_xs))/2, Mathf.Max(t_ys) - (Mathf.Max(t_ys) - Mathf.Min(t_ys))/2);
        this.Rectangle.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(t_xs) - Mathf.Min(t_xs), Mathf.Max(t_ys) - Mathf.Min(t_ys));

        this.DarkPanel.color = new Color32(0, 0, 0, (byte)Random.Range(0, 70));

        string stringToPost = this.generateLabels(t_xs, t_ys);

        System.IO.File.WriteAllText("C:/Work/BP/DatasetGenerator/Dataset Generator/txts/" + this.counter + ".txt", stringToPost);

        ScreenCapture.CaptureScreenshot("C:/Work/BP/DatasetGenerator/Dataset Generator/imgs/" + this.counter + ".png");
        
        this.counter++;

    }

    private string generateLabels(float[] t_xs, float[] t_ys)
    {
        string stringToPost = ((this.counter % 2) + 1) + " " + ((Mathf.Min(t_xs) + (Mathf.Max(t_xs) - Mathf.Min(t_xs)) / 2) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (1.0f - ((Mathf.Max(t_ys) - (Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2) / this.Background.GetComponent<RectTransform>().rect.height)).ToString().Replace(',', '.') + " "
            + ((Mathf.Max(t_xs) - Mathf.Min(t_xs)) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + ((Mathf.Max(t_ys) - Mathf.Min(t_ys)) / this.Background.GetComponent<RectTransform>().rect.height).ToString().Replace(',', '.') + '\n';
        
        if (this.counter % 2 == 0)
        {
            stringToPost += 3 + " " + ((Mathf.Min(t_xs)) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (1.0f - ((Mathf.Min(t_ys)) / this.Background.GetComponent<RectTransform>().rect.height)).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_xs) - Mathf.Min(t_xs)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.height).ToString().Replace(',', '.') + '\n';

            stringToPost += 3 + " " + ((Mathf.Min(t_xs)) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (1.0f - ((Mathf.Max(t_ys)) / this.Background.GetComponent<RectTransform>().rect.height)).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_xs) - Mathf.Min(t_xs)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.height).ToString().Replace(',', '.') + '\n';

            stringToPost += 3 + " " + ((Mathf.Max(t_xs)) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (1.0f - ((Mathf.Min(t_ys) - (Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2) / this.Background.GetComponent<RectTransform>().rect.height)).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_xs) - Mathf.Min(t_xs)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.height).ToString().Replace(',', '.') + '\n';

            stringToPost += 3 + " " + ((Mathf.Max(t_xs)) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (1.0f - ((Mathf.Max(t_ys) - (Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2) / this.Background.GetComponent<RectTransform>().rect.height)).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_xs) - Mathf.Min(t_xs)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.width).ToString().Replace(',', '.') + " "
            + (((Mathf.Max(t_ys) - Mathf.Min(t_ys)) / 2.5f) / this.Background.GetComponent<RectTransform>().rect.height).ToString().Replace(',', '.');
        }

        return stringToPost;
    }

    #endregion Private methods
}
