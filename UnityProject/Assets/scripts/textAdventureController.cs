using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class textAdventureController : MonoBehaviour {

    public InputField input;
    InputField.SubmitEvent se;
    public Text output;
    List<string> consoleHistory;

    public List<string> genericAnswers;

    public string fieldLocked;
    public string youHaveAKey;
    public string wallInWay;
    public string won;
    public string help;

    public bool hasKey;

    public mapField[][] map;

    public int playerPosX;
    public int playerPosY;

    private int wrongAnswers;

    private int historyIndex;

	// Use this for initialization
	void Start () {
        historyIndex = 0;
        consoleHistory = new List<string>();

        initMap();
        writeHelp();

        hasKey = false;

        genericAnswers.Add("What do you say???");
        genericAnswers.Add("huh?");
        genericAnswers.Add("i wouldn't do that dave!");
        genericAnswers.Add("nothing to do there!");

        playerPosX = 2;
        playerPosY = 2;
        wrongAnswers = 0;

        input.ActivateInputField();
        se = new InputField.SubmitEvent();
        se.AddListener(SubmitInput);
        input.onEndEdit = se;
	}

    void initMap()
    {
        map = new mapField[3][];
        for (int i = 0 ; i < 3 ; i++)
        {
            map[i] = new mapField[3];
        }

        map[0][0] = new mapField("no", true, "Cell", "maybe go north?");
        map[1][0] = new mapField("key", true, "Cell", "maybe go west?");
        map[1][1] = new mapField("no", true, "Cell", "maybe go west?");
        map[2][0] = new mapField("no", true, "Cell", "maybe go north?");
        map[0][1] = new mapField("exit", false, "Hallway", "maybe go north?");
        map[1][1] = new mapField("no", false, "Hallway", "maybe go north?");
        map[2][1] = new mapField("no", true, "Hallway", "maybe go north?");
        map[0][2] = new mapField("no", true, "Cell", "maybe go north?");
        map[1][2] = new mapField("no", true, "Cell", "maybe go north?");
        map[2][2] = new mapField("newspaper", false, "Cell", "maybe go north?");
    }

    private void SubmitInput(string currText)
    {
        // Add to History
        consoleHistory.Add(currText);
        historyIndex = consoleHistory.Count - 1;

        // reset Input
        input.ActivateInputField();
        input.text = "";
        // Debug.Log("History Text: " + currText + " / History Index: " + historyIndex);
        ProcessInput(currText);
    }

    void Update()
    {
        HandleInputs();
    }

    void writeHelp()
    {
        help = "go north \ngo east \ngo south \ngo west \n\nlook around \npickup";
    }

    void ProcessInput(string command)
    {
        Debug.Log("X: " + playerPosX + " // Y: " + playerPosY);

        // Process input command - only one base command and one parameter possible
        string[] commandArray = command.Split(' ');
        if(commandArray.Length > 0 && command != "")
        {
            string baseCommand = commandArray[0];
            string parameter = "";
            if (commandArray.Length > 1)
            {
                parameter = commandArray[1];
            }

            switch (baseCommand)
            {
                case "look":
                    switch (parameter)
                    {
                        case "around":
                            if (playerPosY < 1)
                            {
                                output.text = "in the north there is a wall.";
                            }
                            else
                            {
                                output.text = "in the north there is a " + map[playerPosX][playerPosY - 1].name;
                            }

                            if (playerPosX == map.Length - 1)
                            {
                                output.text += "\nin the east there is a wall.";
                            }
                            else
                            {
                                output.text += "\nin the east there is a " + map[playerPosX + 1][playerPosY].name;
                            }

                            if (playerPosY == map[0].Length - 1)
                            {
                                output.text += "\nin the south there is a wall.";
                            }
                            else
                            {
                                output.text += "\nin the south there is a " + map[playerPosX][playerPosY + 1].name;
                            }

                            if (playerPosX < 1)
                            {
                                output.text += "\nin the weast there is a wall.";
                            }
                            else
                            {
                                output.text += "\nin the west there is a " + map[playerPosX - 1][playerPosY].name;
                            }
                            break;
                    }

                    break;
                case "pickup":
                    switch (parameter)
                    {
                        case "newspaper":
                            if(map[playerPosX][playerPosY].item == "newspaper")
                            {
                                output.text = "there was a key in the newspaper! it is now in your inventory.";
                                hasKey = true;
                            }
                            else
                            {
                                output.text = "no nowspaper here!";
                            }
                            break;
                        default:
                            output.text = "there is no " + parameter + " to pick up!";
                            break;
                    }
                    break;
                case "go":
                    mapField target;
                    int targetX = playerPosX;
                    int targetY = playerPosY;
                    bool parameterValid = true;
                    switch (parameter)
                    {
                        case "north":
                            targetY--;
                            break;
                        case "south":
                            targetY++;
                            break;
                        case "east":
                            targetX++;
                            break;
                        case "west":
                            targetX--;
                            break;
                        default:
                            parameterValid = false;
                            break;
                    }
                    Debug.Log("target X: " + targetX + " // target Y: " + targetY);


                    if (parameterValid)
                    {
                        // Super ugly winning condition
                        if (playerPosX == 0 && playerPosY == 1 && parameter == "west")
                        {
                            output.text = won;
                        }
                        else
                        {
                            if (CheckBoundries(targetX, targetY))
                            {
                                target = map[targetX][targetY];
                                if (target.locked)
                                {
                                    if (hasKey)
                                    {
                                        output.text = youHaveAKey;
                                        output.text += "\n" + YouWent(target.name, parameter);
                                        playerPosX = targetX;
                                        playerPosY = targetY;
                                    }
                                    else
                                    {
                                        output.text = fieldLocked;
                                    }
                                }
                                else
                                {
                                    output.text = YouWent(target.name, parameter);
                                    playerPosX = targetX;
                                    playerPosY = targetY;
                                }
                            }
                            else
                            {
                                output.text = wallInWay;
                            }
                        }
                    }
                    else
                    {
                        output.text = baseCommand + " funktioniert nicht mir diesem Parameter!";
                    }

                    break;
                case "help":
                    output.text = help;
                    break;

                default:
                    wrongAnswers++;
                    if (wrongAnswers > 3)
                    {
                        output.text = map[playerPosX][playerPosY].hint;
                    }
                    else
                    {
                        output.text = genericAnswers[UnityEngine.Random.Range(0, genericAnswers.Count - 1)];

                    }
                    break;
            }
        }
        output.text += "\nYou Typed: " + command;
    }

    bool CheckBoundries(int posX, int posY)
    {
        bool isInBoundries = false;
        if (posX < map.Length && posX >= 0 && posY < map[0].Length && posY >= 0)
        {
            isInBoundries = true;
        }
        return isInBoundries;
    }

    string YouWent(string name, string direction)
    {
        return output.text = "You went " + direction + " in a " + name + ".";
    }

    void HandleInputs()
    {

        // History Up
        if (Input.GetKeyDown("up"))
        {
            if(historyIndex < 0)
            {
                historyIndex = consoleHistory.Count - 1;
            }
            input.text = consoleHistory[historyIndex];
            historyIndex--;
        }

        // History Down
        if (Input.GetKeyDown("down"))
        {
            if (historyIndex >= consoleHistory.Count)
            {
                historyIndex = 0;
            }
            input.text = consoleHistory[historyIndex];
            historyIndex++;
        }
    }

    public class mapField
    {
        public bool locked;
        public string item;
        public string name;
        public string hint;
        public mapField(string it, bool lkd, string currName, string currHint)
        {
            item = it;
            locked = lkd;
            name = currName;
            hint = currHint;
        }
    }
}
