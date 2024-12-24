using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Particle
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
    [RequireComponent(typeof(WaterTriggerHandler))]
    public class InteractableWater : MonoBehaviour
    {
        [Header("メッシュ")]
        [Range(2, 500)] public int NumOfXVertices = 70;
        public float Width = 10;
        public float Height = 4;
        public Material WaterMaterial;
        private const int NUM_OF_Y_VERTICES = 2;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Vector3[] _vertices;
        private int[] _topVerticesIndex;

        private EdgeCollider2D _coll;

        private void Reset()
        {
            _coll = GetComponent<EdgeCollider2D>();
            _coll.isTrigger = true;
        }
    
        public void ResetEdgeCollider()
        {
            _coll = GetComponent<EdgeCollider2D>();
            var newPoints = new Vector2[2];
            var firstPoint = new Vector2(_vertices[_topVerticesIndex[0]].x, _vertices[_topVerticesIndex[0]].y);
            newPoints[0] = firstPoint;
        
            var secondPoint = new Vector2(_vertices[_topVerticesIndex[^1]].x, _vertices[_topVerticesIndex[^1]].y);
            newPoints[1] = secondPoint;
        
            _coll.points = newPoints;
            _coll.offset = Vector2.zero;
        }

        public void GenerateMesh()
        {
            _mesh = new Mesh();
        
            _vertices = new Vector3[NumOfXVertices * NUM_OF_Y_VERTICES];
            _topVerticesIndex = new int[NumOfXVertices];
            for(var y = 0; y < NUM_OF_Y_VERTICES; y++)
            {
                for (var x = 0; x < NumOfXVertices; x++)
                {
                    var xPos = (x / (float)(NumOfXVertices - 1)) * Width - Width / 2;
                    var yPos = (y / (float)(NUM_OF_Y_VERTICES - 1)) * Height - Height / 2;
                    _vertices[y * NumOfXVertices + x] = new Vector3(xPos, yPos, 0.0f);
                    if(y == NUM_OF_Y_VERTICES - 1)
                        _topVerticesIndex[x] = y * NumOfXVertices + x;
                }
            }
        
            var triangles = new int[(NumOfXVertices - 1) * (NUM_OF_Y_VERTICES - 1) * 6];
            var index = 0;
        
            for(var y = 0; y < NUM_OF_Y_VERTICES - 1; y++)
            {
                for (var x = 0; x < NumOfXVertices - 1; x++)
                {
                    var bottomLeft = y * NumOfXVertices + x;
                    var bottomRight = bottomLeft + 1;
                    var topLeft = bottomLeft + NumOfXVertices;
                    var topRight = topLeft + 1;
                
                    triangles[index++] = bottomLeft;
                    triangles[index++] = topLeft;
                    triangles[index++] = bottomRight;
                
                    triangles[index++] = bottomRight;
                    triangles[index++] = topLeft;
                    triangles[index++] = topRight;
                }
            }
        
            var uvs = new Vector2[_vertices.Length];
            for (var i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2((_vertices[i].x / Width / 2), (_vertices[i].y + Height / 2) / Height);
            }
        
            if(_meshFilter == null)
                _meshFilter = GetComponent<MeshFilter>();
            if(_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
        
            _meshRenderer.material = WaterMaterial;
            _mesh.vertices = _vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uvs;
        
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshFilter.mesh = _mesh;
        }

        private void Start()
        {
            GenerateMesh();
        }
    }


    [CustomEditor(typeof(InteractableWater))]
    public class InteractableWaterEditor : Editor
    {
        private InteractableWater _water;
    
        public void OnEnable()
        {
            _water = target as InteractableWater;
        }
    
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
        
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
        
            root.Add(new VisualElement { style = { height = 10 } });
        
            var generateMeshButton = new Button(() => _water.GenerateMesh()) { text = "Generate Mesh" };
            root.Add(generateMeshButton);
        
            var resetEdgeColliderButton = new Button(() => _water.ResetEdgeCollider()) { text = "Reset Edge Collider" };
            root.Add(resetEdgeColliderButton);
        
            return root;
        }
    
        private void ChangeDimensions(ref float width, ref float height, float calculatedWidthMax, float calculatedHeightMax)
        {
            width = Mathf.Max(0.1f, calculatedWidthMax);
            height = Mathf.Max(0.1f, calculatedHeightMax);
        }
    }
}