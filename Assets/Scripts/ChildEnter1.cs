using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ChildEnter1 : MonoBehaviour
{

    [SerializeField] AudioSource audio_sc;
    [SerializeField] AudioClip[] audio_clip;

    [SerializeField] Sprite emptyLives;

    [SerializeField] Image[] lives;

    [SerializeField] TMP_Text text_Score;
    [SerializeField] TMP_InputField childInput;


    [SerializeField] GameObject[] letter;
    [SerializeField] GameObject prefabWord;

    [SerializeField] Transform spawnPositions;

    [SerializeField] Animator showWord;
    [SerializeField] Animator playerFly;

    GameObject instantiatedWord;

    private int Audioindex;
    private int letterCheckIndex;
    private int score;
    private int typedWordIndex;
    private int livesIndex;



    public void Start()
    {

        Audioindex = 0;
        letterCheckIndex = 0;
        score = 0;
        livesIndex = 0;


        typedWordIndex = 0;
        // particle = GetComponent<ParticleSystem>();
        AssignWords();
        StartCoroutine(ShowWithDelay());
        childInput.onValueChanged.AddListener(delegate { InputCheck(); });
    }


    public void InputAudio()
    {
        audio_sc.Play();
    }
    public void AppreciateAudio()
    {
        audio_sc.clip = audio_clip[Audioindex];
        audio_sc.Play();
        Audioindex++;
        if (Audioindex == 5)
        {
            Audioindex = 0;
        }

    }

    public void AssignWords()
    {
        //looping through wordlist
        for (int i = 0; i < TeacherEnter.wordList.Count; i++)
        {
            string word = TeacherEnter.wordList[i];
            instantiatedWord = Instantiate(prefabWord, spawnPositions);
            //looping through each letter in word
            for (int j = 0; j < word.Length; j++)
            {

                int index = CheckLetter(word[j]);
                Debug.Log("i " + i);
                Debug.Log("j " + j);
                GameObject instantiatedLetter = Instantiate(letter[index], instantiatedWord.transform.GetChild(j));
                instantiatedWord.transform.GetChild(j).gameObject.SetActive(true);
            }
            instantiatedWord.SetActive(false);

        }
    }

    public int CheckLetter(char c)
    {
        int returnValue = 0;
        for (int k = 0; k < letter.Length; k++)
            if (c == char.Parse(letter[k].name))

                returnValue = k;


        return returnValue;
    }

    //word show one by one
   /* IEnumerator ShowWithDelay()
    {

        for (int i = typedWordIndex; i < TeacherEnter.wordList.Count; i++)
        {
            yield return new WaitForSeconds(3f);
            spawnPositions.transform.GetChild(typedWordIndex).gameObject.SetActive(true);

            yield return new WaitForSeconds(15f);
            childInput.text = "";
        }
    }*/

        /*        while (typedWordIndex <= TeacherEnter.wordList.Count)
                {

                    Debug.Log("showwithdelay called " + typedWordIndex);
                    //showWord.SetTrigger("show");

                    yield return new WaitForSeconds(2f);
                    //playerFly.SetTrigger("playerFly");

                    yield return new WaitForSeconds(3f);
                    spawnPositions.transform.GetChild(typedWordIndex).gameObject.SetActive(true);

                    yield return new WaitForSeconds(15f);
                    childInput.text = "";
                }*/

 

    //each letter hiding
    IEnumerator HideLetter()
    {

        Debug.Log("particles playing");
        spawnPositions.transform.GetChild(typedWordIndex).GetChild(letterCheckIndex).GetChild(0).GetChild(1).GetComponentInChildren<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);

        //  spawnPositions.GetComponentInChildren<ParticleSystem>().Play();
        spawnPositions.GetComponentInChildren<LetterPrefabController>().gameObject.SetActive(false);

        // Invoke("HideLetterAfterParticle", 0.2f);
    }


    /* public void HideLetterAfterParticle()
     {
         spawnPositions.GetComponentInChildren<LetterPrefabController>().gameObject.SetActive(false);
     }*/
    //hide prefabword
    IEnumerator HideWord()
    {

        yield return new WaitForSeconds(1f);

        spawnPositions.transform.GetChild(typedWordIndex).gameObject.SetActive(false);
    }



    /* public void InputCheck()
     {

         InputAudio();
         if (childInput.text.Length > 0)
         {

             GameObject go = spawnPositions.GetComponentInChildren<LetterPrefabController>().gameObject;

             if (childInput.text[letterCheckIndex] == go.name[0])
             {
                 //typing matches with the letter

                 StartCoroutine(HideLetter());
                 letterCheckIndex++;
                 Debug.Log("lettercheckIndex" + letterCheckIndex);
                 if (childInput.text.Length == TeacherEnter.wordLength[typedWordIndex])
                 {
                     //word typed fully

                     //call hideword with delay
                     StartCoroutine(HideWord());
                     AppreciateAudio();
                     UpdateScore();

                     typedWordIndex++;
                     Debug.Log("length got");

                     childInput.text = "";
                     letterCheckIndex = 0;

                     StopAllCoroutines();
                     StartCoroutine(ShowWithDelay());


                 }

             }
         }
     }*/

    public void InputCheck()
    {

        InputAudio();
        if (childInput.text.Length > 0)
        {
            GameObject go = spawnPositions.GetComponentInChildren<LetterPrefabController>().gameObject;
            Debug.Log(childInput.text[letterCheckIndex]);
            Debug.Log(go.name[0]);

            if (childInput.text[letterCheckIndex] == go.name[0])
            {
                //typing matches with the letter

                StartCoroutine(HideLetter());
                letterCheckIndex++;

                //child emters the text equals to the word letter length
                if (childInput.text.Length == TeacherEnter.wordLength[typedWordIndex])
                {
                    UpdateScore();

                    StartCoroutine(HideWord());
                    childInput.text = "";
                    letterCheckIndex = 0;
                    typedWordIndex++;


                    StartCoroutine(ShowWithDelay());
                   


                }


            }
        }
    }

    //word show one by one
    IEnumerator ShowWithDelay()
    {
        while (typedWordIndex <= TeacherEnter.wordList.Count)
        {
            yield return new WaitForSeconds(2f);

            spawnPositions.transform.GetChild(typedWordIndex).gameObject.SetActive(true);


        }
    }
    public void UpdateScore()
    {
        score++;
        text_Score.text = "Score : " + score;
    }
    public void lossScore()
    {
        score--;
        text_Score.text = "Score : " + score;
    }

    //Hide the word if letter hits the ground
    public void CollisionDeleteWord()
    {
     
        StartCoroutine(HideWord());
        typedWordIndex++;

        childInput.text = "";
        letterCheckIndex = 0;

       // StopAllCoroutines();
        StartCoroutine(ShowWithDelay());
        Debug.Log("i am inside the collision");
    }


}


    /*public void CollisionDeleteWord()
    {

        //StartCoroutine(HideWord());
        typedWordIndex++;
        Debug.Log("collision detected");

        StartCoroutine(HideWord());


        childInput.text = "";
        letterCheckIndex = 0;

   

        StopAllCoroutines();
        StartCoroutine(ShowWithDelay());
    }

}*/

// if (livesIndex < 3)
// {
//     lives[livesIndex].sprite = emptyLives;
//     instantiatedWord.transform.position = spawnPositions.position;
//     Debug.Log( instantiatedWord.transform.position);
//      Debug.Log( spawnPositions.position);
//     livesIndex++;
// }
//showWord.StopPlayback();