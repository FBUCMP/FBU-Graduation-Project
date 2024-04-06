using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    [SerializeField] private List<CursorAnimation> cursorAnimationList;
    private CursorAnimation cursorAnimation;
    private int currentFrame;
    private float frameTimer;
    private int frameCount;
    private int currentCursorIndex;

    public enum CursorType
    {
        Menu,
        InGame
    }
    void Start()
    {
        SetActiveCursorAnimation(cursorAnimationList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorAnimation.doesAnimate && Input.GetMouseButton(0))
        {
            Debug.Log("Mouse Clicked");
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0)
            {
                frameTimer += cursorAnimation.frameRate;
                currentFrame = (currentFrame + 1) % frameCount;
                Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
            }
                
            
        }
        else
        {
            Cursor.SetCursor(cursorAnimation.textureArray[0], cursorAnimation.offset, CursorMode.Auto);
        }
        if(Input.GetMouseButtonDown(1))
        {
            SetActiveCursorAnimation(cursorAnimationList[(currentCursorIndex + 1) % cursorAnimationList.Count]);
        }
        
    }
    
    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation)
    {
        currentCursorIndex = cursorAnimationList.IndexOf(cursorAnimation);
        this.cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate;
        frameCount = cursorAnimation.textureArray.Length;
    }

    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;
        public bool doesAnimate;
    }
}
