using UnityEngine;

public class GameOfLife : MonoBehaviour
{
	public ComputeShader shader;

	private const int Size = 256;
	
	private int kernel;
	private ComputeBuffer bufferA;
	private ComputeBuffer bufferB;
	
	private bool useBufferA = true;
	private Material material;

	private void Awake()
	{
		kernel = shader.FindKernel("game_of_life");
		shader.SetInt("size", Size);

		var random = new System.Random();
		var initialData = new int[Size * Size];
		for (var x = 0; x < Size; x++)
		for (var y = 0; y < Size; y++)
			initialData[x + Size * y] = random.Next(2);

		bufferA = new ComputeBuffer(initialData.Length, 4);
		bufferB = new ComputeBuffer(initialData.Length, 4);
		bufferA.SetData(initialData);
		
		material = GetComponent<MeshRenderer>().material;
		material.SetInt("size", Size);
	}

	private void Update()
	{
		var inputBuffer = useBufferA ? bufferA : bufferB;
		var outputBuffer = useBufferA ? bufferB : bufferA;
		
		material.SetBuffer("buffer", outputBuffer);
		shader.SetBuffer(kernel, "input_buffer", inputBuffer);
		shader.SetBuffer(kernel, "output_buffer", outputBuffer);
		shader.Dispatch(kernel, 
			Size / 8, 
			Size / 8, 
			1);

		useBufferA = !useBufferA;
	}

	private void OnDestroy()
	{
		bufferA.Release();
		bufferA.Dispose();
		
		bufferB.Release();
		bufferB.Dispose();
	}
}