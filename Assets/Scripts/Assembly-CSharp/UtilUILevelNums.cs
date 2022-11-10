using System;
using UnityEngine;

public class UtilUILevelNums : MonoBehaviour
{
	private float scale;

	private float xSpeed;

	private float ySpeed;

	private float ya;

	private float time;

	private DampedVibration m_dampedVibration = new DampedVibration();

	private void Start()
	{
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			xSpeed = UnityEngine.Random.Range(30, 45);
		}
		else
		{
			xSpeed = UnityEngine.Random.Range(-45, -30);
		}
		ySpeed = 300f;
		ya = 600f;
		time = 1.2f;
		m_dampedVibration.SetParameter(1.8f, 3f, (float)Math.PI * 5f / 6f, (float)Math.PI / 2f);
	}

	private void Update()
	{
		time -= Time.deltaTime;
		if (time < 0f)
		{
			base.gameObject.SetActiveRecursively(false);
			return;
		}
		ySpeed -= ya * Time.deltaTime;
		base.transform.localPosition = new Vector3(base.transform.localPosition.x + xSpeed * Time.deltaTime, base.transform.localPosition.y + ySpeed * Time.deltaTime, base.transform.localPosition.z);
		float num = m_dampedVibration.CalculateDistance(1.2f - time);
		base.gameObject.transform.localScale = new Vector3(1f - num, 1f - num, 1f - num);
		Color color = base.gameObject.GetComponent<TUILabel>().color;
		Color colorBK = base.gameObject.GetComponent<TUILabel>().colorBK;
		base.gameObject.GetComponent<TUILabel>().color = new Color(color.r, color.g, color.b, color.a - Time.deltaTime);
		base.gameObject.GetComponent<TUILabel>().colorBK = new Color(colorBK.r, colorBK.g, colorBK.b, colorBK.a - Time.deltaTime);
	}

	public void SetNums(Nums nums)
	{
		Color color = base.gameObject.GetComponent<TUILabel>().color;
		Color colorBK = base.gameObject.GetComponent<TUILabel>().colorBK;
		base.gameObject.GetComponent<TUILabel>().color = new Color(color.r, color.g, color.b, 1f);
		base.gameObject.GetComponent<TUILabel>().colorBK = new Color(colorBK.r, colorBK.g, colorBK.b, 1f);
		base.gameObject.GetComponent<TUILabel>().Text = nums.num.ToString();
		base.gameObject.transform.localPosition = new Vector3(nums.loc.x, nums.loc.y, 0f);
		base.gameObject.SetActiveRecursively(true);
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			xSpeed = UnityEngine.Random.Range(20, 40);
		}
		else
		{
			xSpeed = UnityEngine.Random.Range(-40, -20);
		}
		ySpeed = 300f;
		ya = 600f;
		time = 1.2f;
		base.gameObject.transform.localScale = Vector3.one;
	}
}
