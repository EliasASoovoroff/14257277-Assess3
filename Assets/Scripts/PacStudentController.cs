using System;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
	// Duration (in seconds) taken to move between two tiles
	const float MovementDuration = 1.0f;

	static readonly int[,] levelMap = {
		{1,2,2,2,2,2,2,2,2,2,2,2,2,7},
		{2,5,5,5,5,5,5,5,5,5,5,5,5,4},
		{2,5,3,4,4,3,5,3,4,4,4,3,5,4},
		{2,6,4,0,0,4,5,4,0,0,0,4,5,4},
		{2,5,3,4,4,3,5,3,4,4,4,3,5,3},
		{2,5,5,5,5,5,5,5,5,5,5,5,5,5},
		{2,5,3,4,4,3,5,3,3,5,3,4,4,4},
		{2,5,3,4,4,3,5,4,4,5,3,4,4,3},
		{2,5,5,5,5,5,5,4,4,5,5,5,5,4},
		{1,2,2,2,2,1,5,4,3,4,4,3,0,4},
		{0,0,0,0,0,2,5,4,3,4,4,3,0,3},
		{0,0,0,0,0,2,5,4,4,0,0,0,0,0},
		{0,0,0,0,0,2,5,4,4,0,3,4,4,0},
		{2,2,2,2,2,1,5,3,3,0,4,0,0,0},
		{0,0,0,0,0,0,5,0,0,0,4,0,0,0},
	};
	
	private enum Direction { NONE, UP, DOWN, LEFT, RIGHT }
	
	private static Vector2Int ToRelative(Direction dir) => dir switch
    {
		Direction.NONE  => Vector2Int.zero,
        Direction.UP    => new Vector2Int(0, +1),
		Direction.DOWN  => new Vector2Int(0, -1),
        Direction.LEFT  => new Vector2Int(-1, 0),
        Direction.RIGHT => new Vector2Int(+1, 0),
		_ => throw new ArgumentOutOfRangeException(nameof(dir), $"Unexpected value: {dir}")
    };

	// The world-space position of the top-left grid tile
	public Vector2 levelOrigin;

	private int GetTileAtLocalPos(Vector3 localPos) {
		if (localPos.y < 0 || localPos.y >= levelMap.GetLength(0))
			throw new ArgumentOutOfRangeException("Invalid local Y-position");
		if (localPos.x < 0 || localPos.x >= levelMap.GetLength(1))
			throw new ArgumentOutOfRangeException("Invalid local X-position");
		return levelMap[(int) localPos.y, (int) localPos.x];
	}

	private int GetTileAtWorldPos(Vector2 worldPos) {
		return GetTileAtLocalPos(worldPos - levelOrigin);
	}

	private Vector2Int GetLocalPos() {
		Vector3 worldPos = transform.position;
		return new Vector2Int((int) worldPos.x, (int) worldPos.y);
	}

	private Vector2 GetWorldPos(Vector2Int localPos) {
		return levelOrigin + new Vector2(localPos.x, -localPos.y);
	}

	// private int GetTileRelative(Direction dir) {
	// 	return GetTileRelative(ToRelative(dir));
	// }

	private struct Tween {
		public Vector2 startPos  { get; }
		public Vector2 targetPos { get; }
		public float   startTime { get; }

		public Tween(Vector2 startPos, Vector2 targetPos, float startTime) {
			this.startPos  = startPos;
			this.targetPos = targetPos;
			this.startTime = startTime;
		}

		public float GetProgress(float currTime) =>
			Mathf.Clamp((currTime - startTime) / MovementDuration, 0f, 1f);

		public Vector2 Evaluate(float currTime) =>
			Vector2.Lerp(startPos, targetPos, GetProgress(currTime));

		public bool IsFinished(float currTime) =>
			GetProgress(currTime) == 1f;

		// Convenience getters
		public float Progress => GetProgress(Time.time);
		public bool Finished => IsFinished(Time.time);

		public override string ToString() => $"({startPos}, {targetPos}, {startTime})";
	}

	private Tween tween;
	private Direction lastInput;
	private Direction currentDir;

	void Start() {
		
	}

	void Update() {
		if (currentDir != Direction.NONE) {
			transform.position = tween.Evaluate(Time.time);
			if (tween.Finished) currentDir = Direction.NONE;
		}
		UpdateInput();
		Debug.Log(lastInput);
		if (currentDir == Direction.NONE) {
			Vector2Int localPos = GetLocalPos();	   // Snap position to grid.
			Vector2 startPos = GetWorldPos(localPos);  // Avoids precision issues.
			Vector2 endPos = startPos + ToRelative(lastInput);
			tween = new Tween(startPos, endPos, Time.time);
			currentDir = lastInput;
		}
	}

	private void UpdateInput() {
		if (Input.GetKeyDown(KeyCode.W))
			lastInput = Direction.UP;
		if (Input.GetKeyDown(KeyCode.A))
			lastInput = Direction.LEFT;
		if (Input.GetKeyDown(KeyCode.S))
			lastInput = Direction.DOWN;
		if (Input.GetKeyDown(KeyCode.D))
			lastInput = Direction.RIGHT;
	}
}
