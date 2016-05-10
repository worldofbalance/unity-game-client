using UnityEngine;
using System.Collections;
using System;

// Method to get Credits via username or user ID

public class DemGetCredit : MonoBehaviour
{

    public string[] credit;
    public string pCredit;
    public int strLen;

    // Use this for initialization
    IEnumerator Start()
    {
        // probably the wrong way, but it works... good enough for me.
        WWW playerCredit = new WWW("http://thecity.sfsu.edu/~suwu/csc631/credits.php");
        yield return playerCredit;
        string playerCreditString = playerCredit.text;
        credit = playerCreditString.Split(';'); // username, id, credits

    }

    string getValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index));
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }

    string getCredit(int index)
    {
        pCredit = getValue(credit[index], "Credits:");
        strLen = pCredit.Length;
        return pCredit.Substring(8);
    }

    string getCreditByUsername(string username)
    {
        int index = 0;
        int dataLen = credit.Length;

        string user;

        while (index <= dataLen)
        {
            user = getValue(credit[index], "Name:");
            strLen = user.Length;
            user = user.Substring(5);
            if (user.Equals(username))
            {
                //print(credit[index]);
                return getCredit(index);
            }
            index++;
        }
        return "null";  //username not found
    }

    string getCreditByUserID(int ID)
    {
        int index = 0;
        int dataLen = credit.Length;

        string user;
        int userID;

        while (index <= dataLen)
        {
            user = getValue(credit[index], "ID:");
            strLen = user.Length;
            user = user.Substring(3);
            userID = Convert.ToInt32(user);

            if (userID == ID)
            {
                //print(credit[index]);
                return getCredit(index);
            }
            index++;
        }
        return "null"; //ID not found
    }
}
