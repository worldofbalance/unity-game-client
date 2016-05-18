using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class NetworkProtocolTableBase
{

    private static Dictionary<short, Type> table = new Dictionary<short, Type>();

    protected NetworkProtocolTableBase()
    {
        Init();
    }

    protected abstract void Init();

    public void Add(short protocol_id, string name)
    {
        Type type = Type.GetType(name + "Protocol");

        if (type != null)
        {
            if (!table.ContainsKey(protocol_id))
            {
                table.Add(protocol_id, type);
            }
            else
            {
                Debug.LogError("Protocol ID " + protocol_id + " already exists! Ignored " + name);
            }
        }
        else
        {
            Debug.LogError(name + " not found");
        }
    }

    public virtual Type Get(short protocol_id)
    {
        Type type = null;
		
        if (table.ContainsKey(protocol_id))
        {
            type = table[protocol_id];
        }
        else
        {
            Debug.LogError("Protocol [" + protocol_id + "] Not Found");
        }
		
        return type;
    }
}
