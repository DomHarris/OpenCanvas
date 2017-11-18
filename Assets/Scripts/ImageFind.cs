

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFind: MonoBehaviour
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
                                                            {"author","Van"},
                                                            { "description","Interesting description"},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","notclasic" }
                                                        }
                                                    },

                                                    { "womani",
                                                        new Dictionary<string,string> {
                                                            {"name","Women I" },
                                                            {"author","Willem de Koonig"},
                                                            { "description","Interesting description"},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","notclasic" }
                                                        }
                                                    },
                                                    { "thejoyofliferhythm,joiedevivre",
                                                        new Dictionary<string,string> {
                                                            {"name","The Joy of life" },
                                                            {"author","Joie de Vivre"},
                                                            { "description","Interesting description"},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","notclasic" }
                                                        }
                                                    },
                                                    { "bastidedujasdebouffanparispaulcezanne",
                                                        new Dictionary<string,string> {
                                                            {"name","Bastide Du Jas De Bouffan" },
                                                            {"author","Paul Cezanne"},
                                                            { "description","Interesting description"},
                                                            { "interpretation","Interesting Interpretation"},
                                                            {"style","notclasic" }
                                                        }
                                                    }

                                                   };
        
    }
    public ImageInfo UpdateInfo(Upload.AnnotateImageResponses res)
    {
        
        Dictionary<string, string> painting_info;
        //readINfile();

        foreach (var response in res.responses)
            {
                foreach (var annotation in response.logoAnnotations)
                {
                    foreach (var obj_info in database)
                    {
                       if(obj_info.Key.Contains(annotation.description.Replace(" ", "").ToLower()))
                       {
                           UpdateEntries(obj_info.Value);
                           return image_info;
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