using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI text;
    private Image displayImage;

    private int openHash = Animator.StringToHash("Base Layer.UIFill");
    private int closeHash = Animator.StringToHash("Base Layer.UIUnfill");
    private int nameOpenHash = Animator.StringToHash("Base Layer.NameEnter");
    private int nameCloseHash = Animator.StringToHash("Base Layer.NameExit");

    private int exitHash = Animator.StringToHash("Exit");
    private int nameExitHash = Animator.StringToHash("ExitName");

    private float openTime;
    private float openTimer;

    void Init()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
        displayImage = GetComponent<Image>();

        Close();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (openTime != 0.0f && (stateInfo.nameHash == openHash || stateInfo.nameHash == nameOpenHash) &&
            stateInfo.normalizedTime >= 1.0f)
        {
            openTimer += Time.deltaTime;

            if (openTimer >= openTime)
            {
                Close();
                openTime = 0.0f;
                openTimer = 0.0f;
            }
        }

        if ((stateInfo.nameHash == closeHash || stateInfo.nameHash == nameCloseHash) &&
            stateInfo.normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Open(float openTime, string projectileName, Sprite displaySprite)
    {
        if (animator == null || text == null || displayImage == null)
            Init();

        this.openTime = openTime;

        gameObject.SetActive(true);

        if (text == null)
            animator.SetBool(exitHash, false);
        else
        {
            animator.SetBool(nameExitHash, false);
            text.text = projectileName;
        }

        if (name.StartsWith("Unlock Part"))
        {
            displayImage.sprite = displaySprite;
        }
    }

    private void Close()
    {
        if (text == null)
            animator.SetBool(exitHash, true);
        else
            animator.SetBool(nameExitHash, true);
    }
}