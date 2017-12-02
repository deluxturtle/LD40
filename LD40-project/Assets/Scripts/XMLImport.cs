using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/// <summary>
/// Author: Andrew Seba
/// Description: Imports the tiles from tiled format to unity! :D
/// first time making a utility for myself. :p
/// </summary>
public class XMLImport : MonoBehaviour {

    TextAsset xmlFile;
    Sprite[] sprites;
    string spriteSheetName;
    int layerWidth;
    int layerHeight;

    /// <summary>
    /// Uses sprite sheet to import sprites to game engine.
    /// </summary>
    /// <param name="sheet">Sprite Sheet to Use for import.</param>
	public void LoadNewLevel(TextAsset pxmlFile, string pSpriteSheetName)
    {
        ClearLevel();
        xmlFile = pxmlFile;
        spriteSheetName = pSpriteSheetName;

        StartCoroutine("LoadTiles");

    }

    void ClearLevel()
    {
        //Delete everything!
        Debug.Log("Haven't cleared level yet!");
    }

    /// <summary>
    /// Goes though one by one in the layers to import them into sprite objects for unity.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadTiles()
    {
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

            foreach(XmlNode tile in tempNode.SelectSingleNode("tile"))
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


                    horizontalIndex++;
                    if (horizontalIndex % layerWidth == 0)
                    {
                        //Increase our vertical location
                        verticalIndex--;
                        //reset our horizontal location
                        horizontalIndex = 0;
                    }

                }
            }
        }

        yield return null;
    }
}
