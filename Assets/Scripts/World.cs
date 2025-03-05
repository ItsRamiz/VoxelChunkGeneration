using UnityEngine;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }

    public Vector3 vector1 = new Vector3(0.7f, 0, 0);
    public Vector3 vector2 = new Vector3(0.4f, 0.7f, 0);
    public Vector3 vector3 = new Vector3(0, 0, 0.7f);

    public float x_offset;

    public int chunkSize_X = 5;
    public int chunkSize_Y = 5;
    public int chunkSize_Z = 5;

    public float x_padding = 0;
    public float y_padding = 0;
    public float z_padding = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
