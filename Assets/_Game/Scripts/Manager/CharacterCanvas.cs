using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCanvas : MonoBehaviour
{
    [SerializeField] private Transform Character;
    [SerializeField] private SkinController enemySkin;
    [SerializeField] private Image CharacterLevelBG;
    [SerializeField] private TextMeshProUGUI CharacterLevelText;
    [SerializeField] private TextMeshProUGUI CharacterName;

    private Character character;
    private Canvas canvas;
    private PlayerController characterController;
    private EnemyController enemyController;
    private TextMeshProUGUI characterNameTextMeshPro;
    private TextMeshProUGUI characterLevelTextMeshPro;
    
    void Awake()
    {
        characterLevelTextMeshPro = CharacterLevelText.gameObject.GetComponent<TextMeshProUGUI>();
        character = Character.gameObject.GetComponent<Character>();
        characterController = Character.GetComponent<PlayerController>();
        enemyController = Character.GetComponent<EnemyController>();
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        characterNameTextMeshPro = CharacterName.gameObject.GetComponent<TextMeshProUGUI>();

        if (Character.gameObject.CompareTag(Constants.PLAYER))
        {
            characterNameTextMeshPro.text = "You";
            characterNameTextMeshPro.color = Color.black;
            characterLevelTextMeshPro.text = characterController.Level.ToString();
            CharacterLevelBG.color = Color.black;
        }
        else
        {
            Color randomColor = new(Random.value, Random.value, Random.value);
            enemySkin._enemySkin.EnemyColor[enemyController.EnemySkinID].color = randomColor;
            characterNameTextMeshPro.text = enemyController.enemyName.ToString();
            characterNameTextMeshPro.color = randomColor;
            characterLevelTextMeshPro.text = enemyController.Level.ToString();
            CharacterLevelBG.color = randomColor;
        }
    }

    private void Update()
    {
        if (character.IsDeath == true) canvas.enabled = false;
        else canvas.enabled = true;
        if (Character.gameObject.CompareTag(Constants.PLAYER)) characterLevelTextMeshPro.text = characterController.Level.ToString();
        else characterLevelTextMeshPro.text = enemyController.Level.ToString();
    }
    

    void LateUpdate()
    {
        transform.LookAt(new Vector3(GameManagement.Ins.mainCamera.transform.position.x, GameManagement.Ins.mainCamera.transform.position.y, Character.transform.position.z));
        transform.localScale = new Vector3(1 / Character.localScale.x, 1 / Character.localScale.y, 1 / Character.localScale.z);
    }
}