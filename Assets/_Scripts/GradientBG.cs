using UnityEngine;
using UnityEngine.Rendering;

public class GradientBg : MonoBehaviour
{
    void Start()
    {
        // put the backgorund plane in front of camera (you can change this later);
        //this.transform.position = new Vector3(-1.2f, -1.3f, -1f);

        // position and orient main camera and the background plane
        this.transform.rotation = Quaternion.identity;
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.rotation = Quaternion.identity;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;

        //set background's plane transform parent to be the camera's transform, so the plane doesnt move as camera moves
        this.transform.parent = Camera.main.transform;
        // create the background plane
        MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = this.gameObject.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));

        // set the proper renderering order for the background plane
        mat.renderQueue = ((int)RenderQueue.Background);
        mat.color = Color.white;
        mr.material = mat;
        mr.enabled = true;

        // setting the background plane's 4 corner positions ( you def want to change them later )
        Vector3[] BackgroundPlaneVerteices = new Vector3[4];
        BackgroundPlaneVerteices[0] = new Vector3(0, 0, 0) * 0.25f;
        BackgroundPlaneVerteices[1] = new Vector3(1, 0, 0) * 0.25f;
        BackgroundPlaneVerteices[2] = new Vector3(1, 1, 0) * 0.25f;
        BackgroundPlaneVerteices[3] = new Vector3(0, 1, 0) * 0.25f;

        //define the order in which the vertices in the BackgroundPlaneVerteices shoudl be used to draw the triangle
        int[] trianglesArray = new int[6];
        trianglesArray[0] = 0;
        trianglesArray[1] = 1;
        trianglesArray[2] = 2;
        trianglesArray[3] = 2;
        trianglesArray[4] = 3;
        trianglesArray[5] = 0;

        mf.mesh.vertices = BackgroundPlaneVerteices;
        mf.mesh.triangles = trianglesArray;

        // here to create gradient color
        Color[] colors = new Color[mf.mesh.vertices.Length];
        colors[0] = Color.blue;
        colors[1] = Color.blue;
        colors[2] = Color.cyan;
        colors[3] = Color.white;
        mf.mesh.colors = colors;
    }
}
