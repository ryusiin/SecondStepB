using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Class_PathFinder
{
    // :: Activist
    private InHeist_Activist_Map MAPActivist;

    // :: Constructor
    public InHeist_Class_PathFinder() {
        // :: Activist
        this.MAPActivist = GameObject.FindObjectOfType<InHeist_Activist_Map>();
    }

    // :: Constant
    private Vector3[] DIRECTION = new Vector3[6] { new Vector3( -1, 0, 1 ), new Vector3( 0, -1, 1 ), new Vector3( 1, -1, 0 ), new Vector3( 1, 0, -1 ), new Vector3( 0, 1, -1), new Vector3( -1, 1, 0 ) };

    // :: Variable : for Node
    private LinkedList<Vector3> paths;
    private List<Astar_Node> listEmptyTileNode;
    private List<Astar_Node> listOpenNode;
    private Dictionary<Vector3, Astar_Node> dictClosedNode;

    // :: Initialise
    public void Init()
    {
        // :: Use
        this.paths = new LinkedList<Vector3>();
        this.listEmptyTileNode = new List<Astar_Node>();
        this.listOpenNode = new List<Astar_Node>();
        this.dictClosedNode = new Dictionary<Vector3, Astar_Node>();

        // :: Make Node and put it in list
        this.AddNodeList(this.MAPActivist.GetEmptyTile());
    }

    // :: Get
    public void GetListNode()
    {
        foreach(var itm in this.listEmptyTileNode)
        {
            Debug.LogFormat("{0}, {1}, {2}", itm.x, itm.y, itm.z);
        }
    }
    public List<InHeist_Leader_Character> GetEnemyList(EnumAll.eTeam myTeamColor)
    {
        EnumAll.eTeam enemyColor = EnumAll.eTeam.RED;

        if(myTeamColor == EnumAll.eTeam.RED)
        {
            enemyColor = EnumAll.eTeam.BLUE;
        } else if(myTeamColor == EnumAll.eTeam.BLUE)
        {
            enemyColor = EnumAll.eTeam.RED;
        }

        return this.MAPActivist.GetTeam(enemyColor);
    }
    public InHeist_Leader_Tile GetTargetTile(Vector3 tileKey)
    {
        return this.MAPActivist.GetTile(tileKey);
    }
    public Vector3 GetNearTile(Vector3 myTileKey, Vector3 targetTileKey)
    {
        // :: for Use
        List<Vector3> listDirectionTile = new List<Vector3>();

        // :: Get
        for (var i = 0; i < DIRECTION.Length; i++)
        {
            // :: Check Direction with Const
            Vector3 tileKey = new Vector3((int)(targetTileKey.x + DIRECTION[i].x), (int)(targetTileKey.y + DIRECTION[i].y), (int)(targetTileKey.z + DIRECTION[i].z));
            var tile = this.MAPActivist.GetTile(tileKey);
            
            if (tile == null)
                continue;

            if (tile.GetStatus_isEmpty() == true)
                listDirectionTile.Add(tile.Coordinate);
        }

        // :: Check Error
        if(listDirectionTile.Count == 0)
        {
            Vector3 mostNearVector = new Vector3((int)(targetTileKey.x + DIRECTION[0].x), (int)(targetTileKey.y + DIRECTION[0].y), (int)(targetTileKey.z + DIRECTION[0].z));
            for (var i = 1; i < DIRECTION.Length; i++)
            {
                // :: Check Direction with Const
                Vector3 tempVector = new Vector3((int)(targetTileKey.x + DIRECTION[i].x), (int)(targetTileKey.y + DIRECTION[i].y), (int)(targetTileKey.z + DIRECTION[i].z));

                float distanceA = Vector3.Distance(myTileKey, mostNearVector);
                float distanceB = Vector3.Distance(myTileKey, tempVector);

                if (distanceA > distanceB)
                    mostNearVector = tempVector;
            }

            //:: Debug
            Debug.LogFormat("This is Most Near Vector : {0}", mostNearVector);
            return mostNearVector;
        }

        // :: Set
        Vector3 mostNearTile = listDirectionTile[0];

        // :: Find
        for(int i = 1; i < listDirectionTile.Count; i++)
        {
            float distanceA = Vector3.Distance(myTileKey, mostNearTile);
            float distanceB = Vector3.Distance(myTileKey, listDirectionTile[i]);

            if(distanceA > distanceB)
            {
                mostNearTile = listDirectionTile[i];
            }
        }

        // :: Return
        Debug.LogFormat("This is Most Near Tile : {0}", mostNearTile);
        return mostNearTile;
    }

    // :: Make Node
    private void AddNodeList(List<InHeist_Leader_Tile> listTile)
    {
        // :: Convert
        foreach (var itm in listTile)
        {
            // :: Make Node
            Astar_Node node = new Astar_Node();
            node.Init(itm.Coordinate);

            // :: Add it
            this.listEmptyTileNode.Add(node);
        }
    }
    public LinkedList<Vector3> GetNearPath(InHeist_Leader_Character baseCharacter, InHeist_Leader_Tile targetTile)
    {
        // :: 근처로 가는 range를 만들고
        int nearRange = baseCharacter.GetAttackRange() ;
        // :: 근처로 가는 경로를 만들어서
        LinkedList<Vector3> nearPath = new LinkedList<Vector3>();
        // :: 찾아
        while (nearPath == null || nearPath.Count <= 0) {
            // :: 근처로 가는 맵을 만들어
            this.paths = new LinkedList<Vector3>();
            this.listEmptyTileNode = new List<Astar_Node>();
            this.listOpenNode = new List<Astar_Node>();
            this.dictClosedNode = new Dictionary<Vector3, Astar_Node>();

            // :: Make Node and put it in list
            this.AddNodeList(this.MAPActivist.GetEmptyTile());

            // :: 근처로 가는 range는 매번 1 추가시키고
            nearRange += 1;
            if(nearRange >= 7) // :: 만약 맵 길이보다 커지면 그냥 나가버려
            {
                break;
            }

            // :: 그 늘어나는 nearRange로 fakePath를 찾아서
            nearPath = this.GetPath(baseCharacter, targetTile, nearRange); 
        }

        // :: 그리고 그 근처로 가는 경로를 줘
        return nearPath == null || nearPath.Count <= 0 ? null : nearPath;
    }

    // :: GET Path
    public LinkedList<Vector3> GetPath(InHeist_Leader_Character baseCharacter, InHeist_Leader_Tile targetTile, int fakeRange = -1)
    {
        // :: Set
        InHeist_Leader_Tile baseTile = baseCharacter.GetCurrentTile();

        // :: EXIT
        if(baseTile == null || targetTile == null)
        {
            Debug.LogError("에러 baseTile 혹은 targetTile이 없음");
            return null;
        }

        // :: Init paths
        this.paths = new LinkedList<Vector3>();

        // :: Init Base & Target Nodes
        Astar_Node baseNode = new Astar_Node();
        baseNode.Init(baseTile.Coordinate);
        Astar_Node targetNode = new Astar_Node();
        targetNode.Init(targetTile.Coordinate);

        // :: Add Base and Target Node in Empty Tile
        if(listEmptyTileNode.Contains(baseNode) == false)
            this.listEmptyTileNode.Add(baseNode);
        if(listEmptyTileNode.Contains(targetNode) == false)
            this.listEmptyTileNode.Add(targetNode);

        // :: Find Path
        int atk_range = fakeRange <= 0 ? baseCharacter.GetAttackRange() : fakeRange;
        var foundPath = this.FindPath(baseNode, targetNode, atk_range);

        // :: Return Path
        return foundPath;
    }

    // :: Get Path Implement
    private LinkedList<Vector3> FindPath(Astar_Node baseNode, Astar_Node targetNode, int atk_range)
    {
        // :: 우선 첫번째 베이스 노드는 닫자
        baseNode.Closed();
        this.paths.AddFirst(baseNode.Coordinate); // :: 패스에 넣어놓고
        this.dictClosedNode.Add(baseNode.Coordinate, baseNode);

        // :: realCost를 설정하고
        int realCost = 1;

        // :: 아직 path를 못찾았어
        int distance = Controller_Coordinate.GetDistance(baseNode.Coordinate, targetNode.Coordinate);
        while (distance > atk_range)
        {
            // :: 우선 6방향의 Node들을 가져와
            List<Astar_Node> listDirectionNodes = this.FindDirectionNode(baseNode);

            // :: 오픈할 녀석들이 있으면 realCost를 올리고
            if (listDirectionNodes.Count > 0)
                realCost += 1;

            // :: 그것들을 열자
            foreach(var itm in listDirectionNodes)
            {
                itm.Open(targetNode, realCost);
                this.listOpenNode.Add(itm);
            }

            // :: 그런데 Open 노드가 더 이상 없으면? 
            // :: 이건 길을 찾을 수 없다는 거야
            if(this.listOpenNode.Count <= 0)
            {
                // :: 그러니 null을 주자
                return null;
            }

            // :: 그 중에 가장 작은 추정거리 Node를 찾자
            Astar_Node minNode = this.FindNode_MinCost(baseNode);

            // :: 근데 minNode가 없으면?
            if(minNode == null)
            {
                // :: base는 이미 닫혀 있으니 예전 path를 지우고
                this.paths.RemoveLast();

                // :: 예전 것(본래 자기 자신)으로도 못 돌아가는 경우면
                if(this.paths.Count == 0)
                {
                    // :: 못 간다는 거지
                    return null;
                }

                // :: 예전 것을 base로 바꿔서 다시해
                baseNode = this.GetNode(this.paths.Last.Value);
                continue;
            }

            // :: 근데 그 추정거리 Node가 baseNode보다 realCost가 작거나 같으면?
            if(minNode.realCost <= baseNode.realCost)
            {
                // :: 예전 baseNode와 동일하거나 minNode보다 큰 realCost를 가진 것들은 path에서 제거해버리고 
                // :: realCost도 minNode에 맞추자
                List<Vector3> removeItems = new List<Vector3>();
                foreach(var itm in this.paths)
                {
                    var tempNode = this.GetNode(itm);
                    if (minNode.realCost <= tempNode.realCost)
                        removeItems.Add(tempNode.Coordinate);
                }
                foreach(var itm in removeItems)
                {
                    this.paths.Remove(itm);
                }
                realCost = minNode.realCost;

                //Debug.LogFormat("baseNode : {0} minNode : {1}", baseNode.Coordinate, minNode.Coordinate);
            }

            // :: 그걸 path에 넣고
            this.paths.AddLast(minNode.Coordinate);

            // :: 일단 닫자
            minNode.Closed();
            this.dictClosedNode.Add(minNode.Coordinate, minNode);
            this.listOpenNode.Remove(minNode);

            // :: base노드를 minNode로 바꾸고
            baseNode = minNode;

            // :: 거리를 측정해
            distance = Controller_Coordinate.GetDistance(baseNode.Coordinate, targetNode.Coordinate);
            
            // :: 그리고 다시 해 찾을 때까지
        }

        // :: 첫번째인 자기 자신은 다시 지우고
        this.paths.RemoveFirst();

        // :: 보내
        return this.paths;
    }
    
    // :: Find Min Distance
    private Astar_Node FindNode_MinCost(Astar_Node baseNode)
    {
        // :: 우선 null Setting을 하고
        Astar_Node minNode = null;
        
        // :: 찾자
        foreach (var itmNode in this.listOpenNode)
        {
            // :: minNode가 null 이라면 우선 baseNode 거리 안에 있는 Node를 찾아서 그걸 minNode로 써
            if (minNode == null)
            {
                if (Controller_Coordinate.GetDistance(baseNode.Coordinate, itmNode.Coordinate) <= 1)
                    minNode = itmNode;
            }
            else
            {
                // :: minNode가 null이 아니라면 우선 itmNode의 baseNode와의 거리를 재
                // :: 거리 안에 있고 추정 코스트가 더 작으면 이제 그게 minNode야
                if (Controller_Coordinate.GetDistance(baseNode.Coordinate, itmNode.Coordinate) <= 1)
                    if (minNode.estimateCost > itmNode.estimateCost)
                        minNode = itmNode;
            }
        }

        // :: Return
        return minNode;
    }
    // :: Check Direction and Return which isn't null
    private List<Astar_Node> FindDirectionNode(Astar_Node baseNode)
    {
        // :: for Use
        List<Astar_Node> listDirectionNode = new List<Astar_Node>();

        // :: Get
        for (var i = 0; i < DIRECTION.Length; i++)
        {
            Astar_Node node = this.GetNode(baseNode.Coordinate + DIRECTION[i]);

            if (node != null)
                if (this.listOpenNode.Contains(node) == false)
                    if(this.dictClosedNode.ContainsKey(node.Coordinate) == false)
                        listDirectionNode.Add(node);
        }

        // :: Return
        return listDirectionNode;
    }
    // :: Get Node
    private Astar_Node GetNode(Vector3 coordinate)
    {
        return this.listEmptyTileNode.Find(ele => ele.x == coordinate.x && ele.y == coordinate.y && ele.z == coordinate.z);
    }

    // :: Node Status Enum
    private enum eNodeStatus
    {
        NONE,
        OPEN,
        CLOSED
    }
    // :: Node
    private class Astar_Node
    {
        // :: Constructor
        public Astar_Node() { }

        // :: XYZ
        public int x;
        public int y;
        public int z;

        // :: Status
        public eNodeStatus status;

        // :: Cost
        public int heuristicCost;
        public int estimateCost;
        public int realCost;

        // :: Initialise
        public void Init(Vector3 coordinate)
        {
            // :: XYZ
            this.x = (int)coordinate.x;
            this.y = (int)coordinate.y;
            this.z = (int)coordinate.z;

            // :: Status
            this.status = eNodeStatus.NONE;

            // :: Cost
            this.heuristicCost = 0;
            this.estimateCost = this.heuristicCost;
        }
        // :: Chagne Status
        public Astar_Node Open(Astar_Node targetNode, int realCost)
        {
            // :: Don't Open this when Closed
            if (this.status == eNodeStatus.CLOSED)
                return null;

            // :: Open
            this.status = eNodeStatus.OPEN;

            // :: Calculate Cost
            this.heuristicCost = Mathf.Abs(this.x - targetNode.x) + Mathf.Abs(this.y - targetNode.y) + Mathf.Abs(this.z - targetNode.z);
            this.realCost = realCost;
            this.estimateCost = this.realCost + this.heuristicCost;

            // :: Return this
            return this;
        }
        public Astar_Node Closed()
        {
            // :: Close
            this.status = eNodeStatus.CLOSED;

            // :: Return this
            return this;
        }

        public Vector3 Coordinate {
            get
            {
                return new Vector3(x, y, z);
            }
        }
    }
}
