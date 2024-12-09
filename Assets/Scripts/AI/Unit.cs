using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	const float minUpdateTime = .2f;
	const float moveThreshold = .5f;

	public Transform target;
	public float speed = 2;

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
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	// updates the path if target moves
	IEnumerator UpdatePath()
    {
		if(Time.timeSinceLevelLoad < .3f)
        {
			yield return new WaitForSeconds(.3f);

		}
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

		float sqrMoveThreshold = moveThreshold * moveThreshold;
		Vector3 targetPosOld = target.position;
        while (true)
        {
			yield return new WaitForSeconds(minUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude> sqrMoveThreshold)
            {
				PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
				targetPosOld = target.position;
			}
		}
    }

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];
		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;

		}
	}


	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}