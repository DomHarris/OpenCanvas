

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFind : MonoBehaviour
{
    public Upload up;

    private Dictionary<string, Dictionary<string, string>> database;

    public ImageInfo image_info;

    private void Start()
    {
        database = new Dictionary<string, Dictionary<string, string>>{

                                                    { "starrynightvangogh",
                                                        new Dictionary<string,string> {
                                                            {"name","Starry Night" },
                                                            {"author","Vincent Van Gogh"},
                                                            { "description","It depicts the view from the East facing window of his asylum room."},
                                                            { "interpretation","\"And yet once again I allowed myself to be lead astray into reaching for stars that are too big.\" - Van Gogh"},
                                                            {"style","notclassic" }
                                                        }
                                                    },

                                                    { "womani",
                                                        new Dictionary<string,string> {
                                                            {"name","Women I" },
                                                            {"author","Willem de Koonig"},
                                                            { "description","Fascinated by the image of alluring lips in cigarette advertisements, he incorporated collages of that 'T-Zone' in early states of the painting."},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","notclasic" }
                                                        }
                                                    },
                                                    { "thejoyofliferhythm,joiedevivre",
                                                        new Dictionary<string,string> {
                                                            { "name","Rhythm, Joie de Vivre" },
                                                            { "author","Robert Delaunay"},
                                                            { "description","Delaunay painted complementary colors opposite each other to convey life’s pleasures and difficulties."},
                                                            { "interpretation","Interesting Interpretation"},
                                                            { "style","notclasic" }
                                                        }
                                                    },
                                                    { "davinci,leonardomonalisa",
                                                        new Dictionary<string,string> {
                                                            {"name","Mona Lisa" },
                                                            {"author","Leonardo Da Vinci"},
                                                            { "description","- The Mona List is the most parodied work of art in the world!\n\n- The landscape in the background of the painting may have been influenced by Chinese paintings."},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","Renessance" }
                                                        }
                                                    },
                                                    { "visualartsdesignposterillustration",
                                                        new Dictionary<string,string> {
                                                            {"name","Northern Race Meeting" },
                                                            {"author","L S Lowry"},
                                                            { "description","- Lowry tries to show the Northern mindset in his paintings.\n- He was called a genius but he always felt he was a failure because his mother told him so."},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","Renessance" }
                                                        }
                                                    }
                                                   };

    }
    public ImageInfo UpdateInfo(Upload.AnnotateImageResponses res, bool tryWeb = false)
    {

        Dictionary<string, string> painting_info;
        //readINfile();

        foreach (var response in res.responses)
        {
            if (!tryWeb)
            {
                foreach (var annotation in response.logoAnnotations)
                {
                    foreach (var obj_info in database)
                    {
                        if (obj_info.Key.Contains(annotation.description.Replace(" ", "").ToLower()))
                        {
                            UpdateEntries(obj_info.Value);
                            return image_info;
                        }
                    }
                }
            }
            else
            {
                if (response.webDetection != null && response.webDetection.webEntities != null && response.webDetection.webEntities.Count > 0)
                {
                    foreach (var annotation in response.webDetection.webEntities)
                    {
                        foreach (var obj_info in database)
                        {
                            if (obj_info.Key.Contains(annotation.description.Replace(" ", "").ToLower()))
                            {
                                UpdateEntries(obj_info.Value);
                                return image_info;
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    private void UpdateEntries(Dictionary<string, string> entires)
    {
        image_info.author_name = entires["author"];
        image_info.painting_name = entires["name"];
        image_info.description = entires["description"];
        image_info.interpretation = entires["interpretation"];
        image_info.style = entires["style"];
    }


}