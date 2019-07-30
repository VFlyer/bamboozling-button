using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using KModkit;

public class BamboozlingButtonScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMSelectable button;
    public Renderer[] objectID;
    public Material[] objectColours;
    public TextMesh displayText;
    public TextMesh[] buttonText;
    public GameObject matStore;

    private int[] answerKey = new int[2];
    private int[] inputs = new int[2];
    private string[] ech = new string[15] { "WHITE", "RED", "ORANGE", "YELLOW", "LIME", "GREEN", "JADE", "GREY", "CYAN", "AZURE", "BLUE", "VIOLET", "MAGENTA", "ROSE", "BLACK" };
    private int pressCount;
    private int stage = 1;
    private bool danger;
    private bool buttonPressed;
    private IEnumerator textCycle;
    private int[] randomiser = new int[11];
    private string[] message = new string[5];
    private string[] textField = new string[55] {"A LETTER", "A WORD", "THE LETTER", "THE WORD", "1 LETTER", "1 WORD", "ONE LETTER", "ONE WORD",
    "B", "C", "D", "E", "G", "K", "N", "P", "Q", "T", "V", "W", "Y",
    "BRAVO", "CHARLIE", "DELTA", "ECHO", "GOLF", "KILO", "NOVEMBER", "PAPA", "QUEBEC", "TANGO", "VICTOR", "WHISKEY", "YANKEE",
    "COLOUR", "RED", "ORANGE", "YELLOW", "LIME", "GREEN", "JADE","CYAN", "AZURE", "BLUE", "VIOLET", "MAGENTA", "ROSE",
    "IN RED", "IN YELLOW", "IN GREEN", "IN CYAN", "IN BLUE", "IN MAGENTA", "QUOTE", "END QUOTE"};
    private int[][] table = new int[15][] {
    new int[55]{-10,8  ,-8 ,3  ,5  ,-10,-9 ,7  ,3  ,-5 ,1  ,-2 ,6  ,5  ,1  ,-10,7  ,-6 ,-9 ,-7 ,-10,-7 ,-3 ,4  ,-8 ,-1 ,-5 ,4  ,7  ,-8 ,-1 ,6  ,-7 ,10 ,7  ,-4 ,-3 ,7  ,-3 ,-8 ,-6 ,6  ,0  ,4  ,1  ,4  ,-6 ,1  ,7  ,-5 ,8  ,9  ,-8 ,-9 ,7},
    new int[55]{8  ,5  ,2  ,9  ,-3 ,3  ,-9 ,-5 ,2  ,8  ,10 ,3  ,-4 ,2  ,5  ,2  ,9  ,-10,-9 ,10 ,-5 ,-10,7  ,-8 ,8  ,10 ,-4 ,10 ,-2 ,-6 ,-7 ,0  ,9  ,10 ,1  ,0  ,-10,8  ,2  ,-5 ,-7 ,-8 ,-4 ,6  ,3  ,-5 ,-2 ,-10,-8 ,6  ,2  ,6  ,0  ,-8 ,0},
    new int[55]{6  ,-2 ,-6 ,8  ,7  ,9  ,4  ,9  ,-5 ,-1 ,-3 ,8  ,10 ,-9 ,3  ,7  ,3  ,-2 ,-10,-8 ,9  ,2  ,1  ,0  ,5  ,4  ,2  ,-7 ,9  ,-4 ,-7 ,1  ,5  ,1  ,6  ,-2 ,7  ,-6 ,-7 ,10 ,9  ,4  ,-9 ,3  ,-7 ,-3 ,6  ,3  ,-2 ,-10,8  ,0  ,-9 ,9  ,-6},
    new int[55]{10 ,-4 ,-8 ,-1 ,9  ,6  ,10 ,0  ,1  ,-1 ,-10,-9 ,-8 ,3  ,8  ,9  ,-3 ,2  ,3  ,2  ,10 ,5  ,-5 ,-10,9  ,-5 ,0  ,1  ,-9 ,3  ,-8 ,8  ,1  ,2  ,-9 ,-3 ,3  ,6  ,7  ,-4 ,-6 ,9  ,-9 ,-3 ,-5 ,3  ,10 ,-9 ,-3 ,5  ,8  ,-7 ,1  ,-4 ,9},
    new int[55]{6  ,-5 ,-4 ,-3 ,4  ,-9 ,-5 ,-9 ,9  ,-1 ,-10,-2 ,1  ,-5 ,-2 ,-4 ,1  ,6  ,7  ,-4 ,2  ,8  ,-5 ,5  ,9  ,-8 ,-9 ,9  ,4  ,-4 ,3  ,0  ,9  ,-2 ,-8 ,7  ,5  ,-6 ,-1 ,-5 ,-1 ,-8 ,5  ,8  ,-4 ,-7 ,-10,5  ,-2 ,6  ,-5 ,-3 ,-4 ,-2 ,6},
    new int[55]{-5 ,8  ,5  ,-9 ,-6 ,0  ,-9 ,0  ,10 ,-8 ,10 ,-9 ,7  ,9  ,7  ,-10,-6 ,6  ,2  ,-2 ,-4 ,7  ,5  ,5  ,6  ,3  ,-2 ,3  ,0  ,6  ,-1 ,7  ,10 ,9  ,3  ,-2 ,2  ,10 ,6  ,-5 ,-7 ,-1 ,-8 ,-5 ,6  ,9  ,7  ,-7 ,-1 ,6  ,-2 ,9  ,7  ,5  ,-1},
    new int[55]{6  ,7  ,-1 ,0  ,10 ,9  ,1  ,-8 ,3  ,2  ,9  ,7  ,6  ,1  ,-6 ,-10,1  ,-2 ,-4 ,1  ,6  ,-6 ,-1 ,2  ,9  ,8  ,-4 ,-6 ,-4 ,5  ,10 ,8  ,10 ,-2 ,10 ,5  ,6  ,-8 ,9  ,-1 ,-10,3  ,10 ,0  ,-6 ,-8 ,-3 ,6  ,-9 ,-4 ,-2 ,-8 ,9  ,-6 ,10},
    new int[55]{3  ,10 ,4  ,10 ,6  ,1  ,8  ,6  ,-4 ,-6 ,5  ,-5 ,9  ,0  ,2  ,-9 ,8  ,8  ,4  ,-4 ,-2 ,-6 ,-7 ,-8 ,5  ,2  ,4  ,-2 ,-8 ,9  ,-9 ,7  ,4  ,8  ,-9 ,3  ,-4 ,-5 ,2  ,6  ,-3 ,1  ,10 ,8  ,-4 ,-10,-8 ,-3 ,7  ,8  ,-7 ,-6 ,3  ,10 ,3},
    new int[55]{7  ,8  ,4  ,6  ,-4 ,-9 ,0  ,-7 ,7  ,-9 ,7  ,10 ,5  ,-8 ,-2 ,5  ,7  ,6  ,-1 ,5  ,-1 ,-10,2  ,-6 ,-3 ,-9 ,5  ,1  ,-2 ,9  ,-3 ,9  ,0  ,8  ,7  ,-8 ,-2 ,1  ,7  ,-8 ,-6 ,9  ,0  ,-7 ,4  ,2  ,-4 ,9  ,2  ,-6 ,4  ,-1 ,8  ,0  ,6},
    new int[55]{3  ,7  ,-5 ,-2 ,-6 ,9  ,7  ,-4 ,-5 ,10 ,3  ,8  ,-1 ,-4 ,6  ,4  ,2  ,5  ,7  ,10 ,3  ,-8 ,7  ,-8 ,7  ,-3 ,5  ,0  ,-6 ,2  ,-4 ,-10,8  ,9  ,8  ,5  ,-6 ,-4 ,2  ,6  ,7  ,9  ,0  ,8  ,6  ,-5 ,7  ,1  ,8  ,-4 ,8  ,10 ,-1 ,-8 ,6},
    new int[55]{-4 ,-8 ,8  ,7  ,-5 ,0  ,-3 ,-9 ,-10,-4 ,6  ,9  ,-7 ,10 ,-2 ,-4 ,-10,5  ,3  ,5  ,10 ,-2 ,2  ,10 ,-9 ,-4 ,8  ,-4 ,-5 ,-10,7  ,-6 ,6  ,8  ,9  ,1  ,-6 ,1  ,4  ,0  ,-1 ,-3 ,-7 ,-5 ,-1 ,-7 ,7  ,-4 ,5  ,5  ,3  ,7  ,2  ,9  ,-7},
    new int[55]{-9 ,-8 ,2  ,3  ,-2 ,-4 ,8  ,-6 ,8  ,2  ,-5 ,10 ,2  ,-4 ,-5 ,4  ,-5 ,-6 ,9  ,-3 ,-4 ,6  ,-9 ,2  ,3  ,1  ,-7 ,9  ,7  ,8  ,9  ,0  ,-8 ,7  ,0  ,-5 ,0  ,-2 ,3  ,4  ,2  ,-4 ,-1 ,-8 ,3  ,2  ,4  ,8  ,-8 ,-9 ,5  ,-3 ,8  ,-6 ,-3},
    new int[55]{0  ,2  ,7  ,3  ,5  ,-4 ,3  ,9  ,8  ,6  ,2  ,-2 ,-9 ,2  ,-4 ,7  ,9  ,-8 ,-3 ,-8 ,2  ,1  ,-6 ,-7 ,8  ,-3 ,-1 ,9  ,0  ,1  ,4  ,1  ,5  ,3  ,-1 ,-3 ,4  ,3  ,-10,10 ,7  ,3  ,4  ,-4 ,2  ,8  ,-8 ,1  ,-6 ,1  ,3  ,-8 ,6  ,2  ,-5},
    new int[55]{-1 ,1  ,-4 ,-2 ,-7 ,0  ,1  ,2  ,9  ,-2 ,4  ,10 ,-1 ,5  ,-7 ,-6 ,-5 ,3  ,7  ,-5 ,7  ,6  ,-6 ,4  ,-9 ,1  ,-7 ,-5 ,7  ,6  ,-3 ,-8 ,0  ,-5 ,10 ,-9 ,-7 ,1  ,6  ,2  ,8  ,-4 ,5  ,2  ,-4 ,-2 ,-7 ,-8 ,-1 ,-4 ,-2 ,-10,3  ,7  ,-9},
    new int[55]{0  ,-6 ,6  ,0  ,-3 ,-9 ,5  ,1  ,10 ,-9 ,-6 ,2  ,-4 ,3  ,-2 ,1  ,6  ,-10,-4 ,9  ,-7 ,10 ,-8 ,-3 ,1  ,8  ,10 ,4  ,-8 ,7  ,-9 ,4  ,-9 ,-1 ,4  ,3  ,-1 ,-2 ,7  ,-9 ,-1 ,-7 ,-9 ,-10,-6 ,-2 ,1  ,-9 ,4  ,3  ,-6 ,-9 ,4  ,10 ,2}};

    //Logging
    static int moduleCounter = 1;
    int moduleID;
    private bool moduleSolved;

    private void Awake()
    {
        moduleID = moduleCounter++;
        button.OnInteract += delegate () { ButtonOn(); return false;  };
    }

    void Start () {
        matStore.SetActive(false);
        message[1] = "THEN";
        textCycle = TextCycle();
        Reset();
	}

    private void Reset()
    {
        //Picks button colour
        randomiser[0] = UnityEngine.Random.Range(0, 15);
        //Picks text colours
        randomiser[1] = UnityEngine.Random.Range(0, 14);
        randomiser[2] = UnityEngine.Random.Range(0, 14);
        //Picks first two messages
        randomiser[3] = UnityEngine.Random.Range(0, 8);
        randomiser[4] = UnityEngine.Random.Range(0, 8);
        //Picks last two messages
        randomiser[5] = UnityEngine.Random.Range(8, 55);
        randomiser[6] = UnityEngine.Random.Range(8, 55);
        //Picks button text
        randomiser[7] = UnityEngine.Random.Range(0, 55);
        randomiser[8] = UnityEngine.Random.Range(0, 55);
        //Picks the punctuation marks embedding the message
        randomiser[9] = UnityEngine.Random.Range(0, 3);
        //Picks wheteher or not a comma is present
        randomiser[10] = UnityEngine.Random.Range(0, 2);

        //Sets button colour
        objectID[0].material = objectColours[randomiser[0]];
        Debug.LogFormat("[Bamboozling Button #{0}] The button was {1} after {2} reset(s)", moduleID, ech[randomiser[0]], pressCount);
        if (randomiser[0] == 14)
        {
            buttonText[0].color = new Color32(192, 192, 192, 255);
            buttonText[1].color = new Color32(192, 192, 192, 255);
        }
        else
        {
            buttonText[0].color = new Color32(0, 0, 0, 255);
            buttonText[1].color = new Color32(0, 0, 0, 255);
        }

        //Sets messages
        switch (randomiser[9])
        {
            case 0:
                message[0] = textField[randomiser[3]];
                message[4] = textField[randomiser[6]];
                break;
            case 1:
                message[0] = "'" + textField[randomiser[3]];
                message[4] = textField[randomiser[6]] + "'";
                break;
            case 2:
                message[0] = "\"" + textField[randomiser[3]];
                message[4] = textField[randomiser[6]] + "\"";
                break;
        }
        message[2] = textField[randomiser[4]] + ":";
        message[3] = textField[randomiser[5]];
        if (randomiser[10] == 1)
        {
            message[0] = message[0] + ",";
        }
        Debug.LogFormat("[Bamboozling Button #{0}] The full message after {1} reset(s) reads: {2} - {3} - {4} - {5} - {6}", moduleID, pressCount, message[0], message[1], message[2], message[3], message[4]);
        StartCoroutine(textCycle);
        buttonText[0].text = textField[randomiser[7]];
        buttonText[1].text = textField[randomiser[8]];
        Debug.LogFormat("[Bamboozling Button #{0}] The upper text on the button after {1} reset(s) reads: {2}", moduleID, pressCount, buttonText[0].text);
        Debug.LogFormat("[Bamboozling Button #{0}] The lower text on the button after {1} reset(s) reads: {2}", moduleID, pressCount, buttonText[1].text);
        Debug.LogFormat("[Bamboozling Button #{0}] Display 4 had the colour {1} after {2} reset(s)", moduleID, ech[randomiser[1]], pressCount);
        Debug.LogFormat("[Bamboozling Button #{0}] Display 5 had the colour {1} after {2} reset(s)", moduleID, ech[randomiser[2]], pressCount);

        //Checks for the appearance of either button text in the display and if so, skips calculations
        if (randomiser[3] == randomiser[7] || randomiser[4] == randomiser[7] || randomiser[5] == randomiser[7] || randomiser[6] == randomiser[7])
        {
            answerKey[0] = table[randomiser[1]][randomiser[5]] % 10;
            if (answerKey[0] < 0)
            {
                answerKey[0] = answerKey[0] + 10;
            }
            answerKey[1] = answerKey[0];
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button should have been double-pressed when the last digit is {2}", moduleID, pressCount, answerKey[0]);
        }
        else if (randomiser[3] == randomiser[8] || randomiser[4] == randomiser[8] || randomiser[5] == randomiser[8] || randomiser[6] == randomiser[8])
        {
            answerKey[0] = table[randomiser[2]][randomiser[6]] % 10;
            if (answerKey[0] < 0)
            {
                answerKey[0] = answerKey[0] + 10;
            }
            answerKey[1] = answerKey[0];
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button should have been double-pressed when the last digit is {2}", moduleID, pressCount, answerKey[0]);
        }
        //Default calculation
        else
        {
            answerKey[0] = table[randomiser[0]][randomiser[5] - randomiser[3]] + table[14 - randomiser[0]][randomiser[6] - randomiser[4]];
            answerKey[1] = table[randomiser[1]][randomiser[7]] + table[randomiser[2]][randomiser[8]];
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), V1 = ( {2} , {3} ) = {4}", moduleID, pressCount, randomiser[5] - randomiser[3], randomiser[0], table[randomiser[0]][randomiser[5] - randomiser[3]]);
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), V2 = ( {2} , {3} ) = {4}", moduleID, pressCount, randomiser[6] - randomiser[4], 14 - randomiser[0], table[14 - randomiser[0]][randomiser[6] - randomiser[4]]);
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), V3 = ( {2} , {3} ) = {4}", moduleID, pressCount, randomiser[7], randomiser[1] , table[randomiser[1]][randomiser[7]]);
            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), V4 = ( {2} , {3} ) = {4}", moduleID, pressCount, randomiser[8], randomiser[2], table[randomiser[2]][randomiser[8]]);
            if (randomiser[10] == 1)
            {
                int temp = answerKey[0];
                answerKey[0] = answerKey[1];
                answerKey[1] = temp;
            }
            switch (randomiser[9])
            {
                case 0:
                    answerKey[0] = (answerKey[0] + 20) % 10;
                    answerKey[1] = (answerKey[1] + 20) % 10;
                    Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button should have been pressed first when the last digit is {2}, and again when it is {3}",moduleID,pressCount,answerKey[0],answerKey[1]);
                    break;
                case 1:
                    answerKey[0] = ((answerKey[0] + 27) % 9) + 3;
                    answerKey[1] = ((answerKey[1] + 27) % 9) + 3;
                    Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button should have been pressed first when the sum of the last two digits is {2}, and again when it is {3}", moduleID, pressCount, answerKey[0], answerKey[1]);
                    break;
                case 2:
                    answerKey[0] = (((2 * answerKey[0]) + 54) % 9) + 3;
                    answerKey[1] = (((2 * answerKey[1]) + 54) % 9) + 3;
                    Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button should have been pressed first when the sum of the last two digits is {2}, and again when it is {3}", moduleID, pressCount, answerKey[0], answerKey[1]);
                    break;
            }
        }
    }

    void ButtonOn()
    {
        if (moduleSolved == false)
        {
            button.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (buttonPressed == false)
            {
                buttonPressed = true;
                objectID[3].material = objectColours[0];
                StopCoroutine(textCycle);
                displayText.text = String.Empty;
                if (randomiser[3] == randomiser[7] || randomiser[4] == randomiser[7] || randomiser[5] == randomiser[7] || randomiser[6] == randomiser[7] || randomiser[3] == randomiser[8] || randomiser[4] == randomiser[8] || randomiser[5] == randomiser[8] || randomiser[6] == randomiser[8])
                {
                    inputs[0] = (int)(Bomb.GetTime() % 60) % 10;
                    Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched on when the last digit of the timer was {2}", moduleID, pressCount, inputs[0]);
                }
                else
                {
                    switch (randomiser[9])
                    {
                        case 0:
                            inputs[0] = (int)(Bomb.GetTime() % 60) % 10;
                            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched on when the last digit of the timer was {2}", moduleID, pressCount, inputs[0]);
                            break;
                        default:
                            inputs[0] = (int)(Bomb.GetTime() % 60 - (Bomb.GetTime() % 60) % 10) / 10 + (int)(Bomb.GetTime() % 60) % 10;
                            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched on when the sum of the last two digits of the timer was {2}", moduleID, pressCount, inputs[0]);
                            break;
                    }
                }
            }
            else
            {
                buttonPressed = false;
                objectID[3].material = objectColours[14];
                if (randomiser[3] == randomiser[7] || randomiser[4] == randomiser[7] || randomiser[5] == randomiser[7] || randomiser[6] == randomiser[7] || randomiser[3] == randomiser[8] || randomiser[4] == randomiser[8] || randomiser[5] == randomiser[8] || randomiser[6] == randomiser[8])
                {
                    inputs[1] = (int)(Bomb.GetTime() % 60) % 10;
                    Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched off when the last digit of the timer was {2}", moduleID, pressCount, inputs[1]);
                }
                else
                {
                    switch (randomiser[9])
                    {
                        //Last digit of timer
                        case 0:
                            inputs[1] = (int)(Bomb.GetTime() % 60) % 10;
                            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched off when the last digit of the timer was {2}", moduleID, pressCount, inputs[1]);
                            break;
                        //Sum of last two digits of timer
                        default:
                            inputs[1] = (int)(Bomb.GetTime() % 60 - (Bomb.GetTime() % 60) % 10) / 10 + (int)(Bomb.GetTime() % 60) % 10;
                            Debug.LogFormat("[Bamboozling Button #{0}] After {1} reset(s), the button was switched off when the sum of the last two digits of the timer was {2}", moduleID, pressCount, inputs[1]);
                            break;
                    }
                }
                if (inputs[0] == answerKey[0] && inputs[1] == answerKey[1])
                {
                    switch (stage)
                    {
                        case 1:
                            objectID[1].material = objectColours[5];
                            break;
                        case 2:
                            objectID[1].material = objectColours[5];
                            objectID[2].material = objectColours[5];
                            objectID[0].material = objectColours[0];
                            GetComponent<KMBombModule>().HandlePass();
                            moduleSolved = true;
                            buttonText[0].text = "WELL";
                            buttonText[1].text = "DONE";
                            break;
                    }
                    stage++;
                    Audio.PlaySoundAtTransform("InputCorrect", transform);
                    danger = false;
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    if (danger == true)
                    {
                        danger = false;
                        stage = 1;
                        objectID[1].material = objectColours[14];
                        objectID[2].material = objectColours[14];
                    }
                    else
                    {
                        danger = true;
                        switch (stage)
                        {
                            case 2:
                                objectID[1].material = objectColours[3];
                                break;
                            case 3:
                                objectID[1].material = objectColours[3];
                                objectID[2].material = objectColours[3];
                                break;
                        }
                    }
                }
                if (moduleSolved == false)
                {
                    pressCount++;
                    Reset();
                }
            }
        }
    }

    private IEnumerator TextCycle()
    {
        for (int i = 0; i < 6; i++)
        {
            if(i == 3 || i == 4)
            {
                switch (randomiser[i - 2])
                {
                    case 0:
                        displayText.color = new Color32(192, 192, 192, 255);
                        break;
                    case 1:
                        displayText.color = new Color32(192, 0, 0, 255);
                        break;
                    case 2:
                        displayText.color = new Color32(192, 96, 0, 255);
                        break;
                    case 3:
                        displayText.color = new Color32(192, 192, 0, 255);
                        break;
                    case 4:
                        displayText.color = new Color32(96, 192, 0, 255);
                        break;
                    case 5:
                        displayText.color = new Color32(0, 192, 0, 255);
                        break;
                    case 6:
                        displayText.color = new Color32(0, 192, 96, 255);
                        break;
                    case 7:
                        displayText.color = new Color32(96, 96, 96, 255);
                        break;
                    case 8:
                        displayText.color = new Color32(0, 192, 192, 255);
                        break;
                    case 9:
                        displayText.color = new Color32(0, 96, 192, 255);
                        break;
                    case 10:
                        displayText.color = new Color32(0, 0, 192, 255);
                        break;
                    case 11:
                        displayText.color = new Color32(96, 0, 192, 255);
                        break;
                    case 12:
                        displayText.color = new Color32(192, 0, 192, 255);
                        break;
                    case 13:
                        displayText.color = new Color32(192, 0, 96, 255);
                        break;
                }
            }
            else
            {
                displayText.color = new Color32(192, 192, 192, 255);
            }
            if(i == 5)
            {
                i = -1;
                displayText.text = String.Empty;
            }
            else
            {
                displayText.text = message[i];
            }
            yield return new WaitForSeconds(1);
        }
    }

    //twitch plays
    private bool isInputValid(string sn)
    {
        int temp = 0;
        bool preformed = int.TryParse(sn, out temp);
        if(preformed == true && (temp >= 0 && temp <= 59))
        {
            return true;
        }
        return false;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press 13 [Presses the button when the last two digits of the bomb's timer are '##:13'] !{0} dtap 5 [Double taps the button when the last digit of the bomb's timer is '##:#5'] | !{0} reset [Resets the module ENTIRELY]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Debug.LogFormat("[Bamboozling Button #{0}] TP reset called!", moduleID);
            StopCoroutine(textCycle);
            Start();
            yield break;
        }
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 2)
            {
                if (isInputValid(parameters[1]))
                {
                    yield return null;
                    int timepress = 0;
                    int.TryParse(parameters[1], out timepress);
                    if (parameters[1].Length == 1)
                    {
                        Debug.LogFormat("[Bamboozling Button #{0}] Pressing the button at '##:#{1}'!", moduleID, timepress);
                        while ((((int)Bomb.GetTime() % 60) % 10) != timepress) yield return "trycancel The button was not pressed due to a request to cancel.";
                    }
                    else
                    {
                        Debug.LogFormat("[Bamboozling Button #{0}] Pressing the button at '##:{1}'!", moduleID, timepress);
                        while (((int)Bomb.GetTime() % 60) != timepress) yield return "trycancel The button was not pressed due to a request to cancel.";
                    }
                    button.OnInteract();
                }
                else
                {
                    yield return "sendtochat That specified set of digits is invalid!";
                }
            }
            yield break;
        }
        if (Regex.IsMatch(parameters[0], @"^\s*dtap\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 2)
            {
                if (isInputValid(parameters[1]))
                {
                    yield return null;
                    int timepress = 0;
                    int.TryParse(parameters[1], out timepress);
                    if (parameters[1].Length == 1)
                    {
                        Debug.LogFormat("[Bamboozling Button #{0}] Double Tapping the button at '##:#{1}'!", moduleID, timepress);
                        while ((((int)Bomb.GetTime() % 60) % 10) != timepress) yield return "trycancel The button was not double tapped due to a request to cancel.";
                    }
                    else
                    {
                        Debug.LogFormat("[Bamboozling Button #{0}] Double Tapping the button at '##:{1}'!", moduleID, timepress);
                        while (((int)Bomb.GetTime() % 60) != timepress) yield return "trycancel The button was not double tapped due to a request to cancel.";
                    }
                    button.OnInteract();
                    yield return new WaitForSeconds(0.05f);
                    button.OnInteract();
                }
                else
                {
                    yield return "sendtochat That specified set of digits is invalid!";
                }
            }
            yield break;
        }
    }
}
