using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	const float minUpdateTime = .2f;
	const float moveThreshold = .5f;

	public Transform target;
	public float speed = 1;

	Vector3[] path;
	int targetIndex;

	void Start()
	{
		StartCoroutine(UpdatePath());
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath"); // if its already running
			StartCoroutine("FollowPath");
		}
	}

	// updates the path if target moves
	IEnumerator UpdatePath()
    {
		if(Time.timeSinceLevelLoad < .3f) // wait upon first load
        {
			yield return new WaitForSeconds(.3f);

		}
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

		float sqrMoveThreshold = moveThreshold * moveThreshold;
		Vector3 targetPosOld = target.position;
        while (true)
        {
			yield return new WaitForSeconds(minUpdateTime); // min time between path req
            if ((target.position - targetPosOld).sqrMagnitude> sqrMoveThreshold) // if target moved a set distance
            {
				PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
				targetPosOld = target.position;
			}
		}
    }

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0]; // beginning of path
		while (true)
		{
			if (transform.position == currentWaypoint) 
			{
				targetIndex++; // advance to the next waypoint
				if (targetIndex >= path.Length) // no more waypoints
				{
					yield break; // exit coroutine
				}
				currentWaypoint = path[targetIndex];
			}

			// move closer to the currentwaypoint
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null; // move to next frame

		}
	}


	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++) // draws the waypoints not passed
			{
				Gizmos.color = Color.green;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) // line the object is moving along
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else // line between waypoints
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}