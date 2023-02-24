using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private GenerateEnum State;
    
    [Header("Option")]
    [SerializeField] private         bool NodeAsync;
    [SerializeField] private         bool MapAsync;
    [SerializeField] private         bool PlayerAsync;
    [Space()]
    [SerializeField] private   Vector3Int MapSize;

    private IEnumerator _generateNodeCorutine = null;
    private IEnumerator _generateMapCorutine = null;
    private IEnumerator _generatePlayerCorutine = null;

    private List<Node> _nodeList;

    private void OnEnable()
    {
        if(_generateNodeCorutine   == null) _generateNodeCorutine   = GenerateNodeCorutine();
        if(_generateMapCorutine    == null) _generateMapCorutine    = GenerateMapCorutine();
        if(_generatePlayerCorutine == null) _generatePlayerCorutine = GeneratePlayerCorutine();

        ChangeState(GenerateEnum.Node);
    }

    private void ChangeState(GenerateEnum newState)
    { 
        State = newState;

        switch (State)
        {
            case GenerateEnum.None:
                break;
            case GenerateEnum.Node:
                StartCoroutine(_generateNodeCorutine);
                break;
            case GenerateEnum.Map:
                StartCoroutine(_generateMapCorutine);
                break;
            case GenerateEnum.Player:
                StartCoroutine(_generatePlayerCorutine);
                break;
            case GenerateEnum.Init:
                break;
        }
    }

    private IEnumerator GenerateNodeCorutine()
    { 
        Transform nodeParent = new GameObject("NodeParent").transform;
        nodeParent.SetParent(transform);

        Node nodePrefab = new GameObject().AddComponent<Node>();

        _nodeList = new List<Node>();

        if (NodeAsync)
        {
            for (int z = 0; z <= MapSize.z; z += 1)
            {
                for (int y = 0; y <= MapSize.y; y += 1)
                {
                    for (int x = 0; x <= MapSize.x; x += 1)
                    {
                        Node node = Node.Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity, nodeParent);
                        node.gameObject.name = $"node_{x}_{y}_{z}";
                        _nodeList.Add(node);
                        yield return null;
                    }
                }
            }
        }

        else
        {
            for (int z = 0; z <= MapSize.z; z += 1)
            {
                for (int y = 0; y <= MapSize.y; y += 1)
                {
                    for (int x = 0; x <= MapSize.x; x += 1)
                    {
                        Node node = Node.Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity, nodeParent);
                        node.gameObject.name = $"node_{x}_{y}_{z}";
                        _nodeList.Add(node);
                    }
                }
            }
        }

        Destroy(nodePrefab.gameObject);

        ChangeState(GenerateEnum.Map);
    }

    private IEnumerator GenerateMapCorutine()
    { 
        yield return null;
        if(MapAsync)
        { 
            
        }

        else
        { 
            Node bottomLeftNode = _nodeList[0];
            Node bottomRightNode = _nodeList[MapSize.x -1];
            Node TopLeftNode = _nodeList[MapSize.x * (MapSize.z - 1)];
            Node TopRightNode = _nodeList[^1];
        }
        ChangeState(GenerateEnum.Player);
    }

    private IEnumerator GeneratePlayerCorutine()
    {
        yield return null;

        ChangeState(GenerateEnum.Init);
    }
}

public enum GenerateEnum
{ 
    None,
    Node,
    Map,
    Player,
    Init
}
