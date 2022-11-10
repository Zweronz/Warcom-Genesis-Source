using UnityEngine;

public class IABAndroidEventListener : MonoBehaviour
{
	private UIIAP iapUI;

	private void OnEnable()
	{
		IABAndroidManager.billingSupportedEvent += billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent += purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent += purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent += purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent += purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent += purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent += transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent += transactionRestoreFailedEvent;
		iapUI = Camera.main.GetComponent<UIIAP>();
	}

	private void OnDisable()
	{
		IABAndroidManager.billingSupportedEvent -= billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent -= purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent -= purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent -= purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent -= transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent -= transactionRestoreFailedEvent;
	}

	private void billingSupportedEvent(bool isSupported)
	{
		Debug.Log("billingSupportedEvent: " + isSupported);
		if (iapUI == null)
		{
			iapUI = Camera.main.GetComponent<UIIAP>();
		}
		iapUI.HideIndicator();
		UIIAP.bAndroidInitOK = isSupported;
	}

	private void purchaseSignatureVerifiedEvent(string signedData, string signature)
	{
		Debug.Log("purchaseSignatureVerifiedEvent. signedData: " + signedData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(string productId)
	{
		Debug.Log("purchaseSucceededEvent: " + productId);
		if (iapUI == null)
		{
			iapUI = Camera.main.GetComponent<UIIAP>();
		}
		iapUI.HideIndicator();
		iapUI.AndoridIAPBuySuccessed();
	}

	private void purchaseCancelledEvent(string productId)
	{
		Debug.Log("purchaseCancelledEvent: " + productId);
		if (iapUI == null)
		{
			iapUI = Camera.main.GetComponent<UIIAP>();
		}
		iapUI.HideIndicator();
	}

	private void purchaseRefundedEvent(string productId)
	{
		Debug.Log("purchaseRefundedEvent: " + productId);
	}

	private void purchaseFailedEvent(string productId)
	{
		Debug.Log("purchaseFailedEvent: " + productId);
		if (iapUI == null)
		{
			iapUI = Camera.main.GetComponent<UIIAP>();
		}
		iapUI.HideIndicator();
	}

	private void transactionsRestoredEvent()
	{
		Debug.Log("transactionsRestoredEvent");
	}

	private void transactionRestoreFailedEvent(string error)
	{
		Debug.Log("transactionRestoreFailedEvent: " + error);
	}
}
