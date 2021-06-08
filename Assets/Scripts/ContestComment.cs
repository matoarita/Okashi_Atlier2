using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ContestComment
{
    public int ID;
    public int CommentID;
    public string ItemName;
    public int SetID;
    public string Comment_1;
    public string Comment_2;
    public string Comment_3;
    public string Comment_4;


    //ここでリスト化時に渡す引数をあてがいます   
    public ContestComment(int _id, int _commentid, string _itemName, int _setid, string _comment1, string _comment2, string _comment3, string _comment4)
    {
        ID = _id;
        CommentID = _commentid;

        ItemName = _itemName;
        SetID = _setid;

        Comment_1 = _comment1;
        Comment_2 = _comment2;
        Comment_3 = _comment3;
        Comment_4 = _comment4;
    }

}
