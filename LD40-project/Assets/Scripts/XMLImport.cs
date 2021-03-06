﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/// <summary>
/// Author: Andrew Seba
/// Description: Imports the tiles from tiled format to unity! :D
/// first time making a utility for myself. :p
/// </summary>
public class XMLImport : MonoBehaviour {

    //Publics
    [Tooltip("Player prefab to spawn on the spawnpoint.")]
    public GameObject playerPrefab;
    [Tooltip("Goal prefab to mark the end of the game.")]
    public GameObject goalPrefab;
    public GameObject fakePrefab;
    List<GameObject> currentLevelObjects = new List<GameObject>();

    //Privates
    TextAsset xmlFile;
    Sprite[] sprites;
    int layerWidth;
    int layerHeight;

    const int GOAL = 33;
    const int FAKE = 4;
    const int SPAWN = 18;
    

    public IEnumerator DeleteMap()
    {
        while (currentLevelObjects.Count > 0)
        {
            GameObject objToRemove = currentLevelObjects[0];
            currentLevelObjects.Remove(objToRemove);
            Destroy(objToRemove);
            yield return null;
        }
        currentLevelObjects = new List<GameObject>();
    }

    /// <summary>
    /// Goes though one by one in the layers to import them into sprite objects for unity.
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadTiles(TextAsset pxmlFile)
    {
        yield return StartCoroutine(DeleteMap());
        xmlFile = pxmlFile;
        string spriteSheetName = "LevelSpriteSheet";

        try
        {
            sprites = Resources.LoadAll<Sprite>(spriteSheetName);
        }
        catch
        {
            Debug.LogWarning("Couldn't find or load in " + spriteSheetName);
        }

        XmlDocument xmlData = new XmlDocument();
        xmlData.LoadXml(xmlFile.text);

        XmlNodeList layerNames = xmlData.GetElementsByTagName("layer");
        XmlNode tilesetInfo = xmlData.SelectSingleNode("map").SelectSingleNode("tileset");
        float tileWidth = (float.Parse(tilesetInfo.Attributes["tilewidth"].Value) / 16f);
        float tileHeight = (float.Parse(tilesetInfo.Attributes["tileheight"].Value) / 16f);

        //int width = int.Parse(xmlData.SelectSingleNode("map").Attributes["width"].Value);
        //int height = int.Parse(xmlData.SelectSingleNode("map").Attributes["height"].Value);


        //For each layer that exists
        foreach (XmlNode layerInfo in layerNames)
        {
            layerWidth = int.Parse(layerInfo.Attributes["width"].Value);
            layerHeight = int.Parse(layerInfo.Attributes["height"].Value);

            //Pull out the data node
            XmlNode tempNode = layerInfo.SelectSingleNode("data");

            int verticalIndex = layerHeight - 1;
            int horizontalIndex = 0;

            foreach(XmlNode tile in tempNode)
            {
                int spriteValue = int.Parse(tile.Attributes["gid"].Value);

                //if not empty
                if(spriteValue > 0)
                {
                    Sprite[] currentSpriteSheet = sprites;

                    //Create a sprite
                    GameObject tempSprite = new GameObject(layerInfo.Attributes["name"].Value + " <" + horizontalIndex + ", " + verticalIndex + ">");
                    SpriteRenderer spriteRend = tempSprite.AddComponent<SpriteRenderer>();
                    spriteRend.sprite = currentSpriteSheet[spriteValue - 1];
                    tempSprite.transform.position = new Vector3((tileWidth * horizontalIndex), (tileHeight * verticalIndex));
                    //set sorting layer
                    spriteRend.sortingLayerName = layerInfo.Attributes["name"].Value;

                    //set parent
                    GameObject parent = GameObject.Find(layerInfo.Attributes["name"].Value + "Layer");
                    if(parent == null)
                    {
                        parent = new GameObject();
                        parent.name = layerInfo.Attributes["name"].Value + "Layer";
                    }
                    tempSprite.transform.parent = parent.transform;

                    //Do special things!!!
                    if (layerInfo.Attributes["name"].Value == "Background")
                    {

                    }
                    else if (layerInfo.Attributes["name"].Value == "Wall")
                    {
                        tempSprite.AddComponent<BoxCollider2D>();
                    }
                    else if(layerInfo.Attributes["name"].Value == "Object")
                    {
                        switch (spriteValue)
                        {
                            case GOAL:
                                GameObject tempGoal = Instantiate(goalPrefab, tempSprite.transform.position, Quaternion.identity);
                                Destroy(tempSprite);
                                currentLevelObjects.Add(tempGoal);
                                break;
                            case SPAWN:
                                GameObject tempPlayer = Instantiate(playerPrefab, tempSprite.transform.position, Quaternion.identity);
                                Destroy(tempSprite);
                                currentLevelObjects.Add(tempPlayer);
                                break;
                            case FAKE:
                                GameObject tempFake = Instantiate(fakePrefab, tempSprite.transform.position, Quaternion.identity);
                                currentLevelObjects.Add(tempFake);
                                Destroy(tempSprite);
                                break;
                            default:
                                break;
                        }
                    }
                    currentLevelObjects.Add(tempSprite);

                }
                horizontalIndex++;
                if (horizontalIndex % layerWidth == 0)
                {
                    //Increase our vertical location
                    verticalIndex--;
                    //reset our horizontal location
                    horizontalIndex = 0;
                    yield return null; // Take a frame to update UI
                }
            }
        }
        yield return null;
    }
}
