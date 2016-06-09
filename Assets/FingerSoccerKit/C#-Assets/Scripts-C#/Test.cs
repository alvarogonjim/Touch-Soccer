using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

	// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
	public class Test : MonoBehaviour, IStoreListener
	{
		private static IStoreController m_StoreController;          // The Unity Purchasing system.
		private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

		// Product identifiers for all products capable of being purchased: 
		// "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
		// counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
		// also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

		// General product identifiers for the consumable, non-consumable, and subscription products.
		// Use these handles in the code to reference which product to purchase. Also use these values 
		// when defining the Product Identifiers on the store. Except, for illustration purposes, the 
		// kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
		// specific mapping to Unity Purchasing's AddProduct, below.
		public static string kProductIDConsumable =    "consumable";   
		public static string kProductIDNonConsumable = "nonconsumable";
		public static string kProductIDSubscription =  "subscription";

	
		// Apple App Store-specific product identifier for the subscription product.
		private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

		// Google Play Store-specific product identifier subscription product.
		private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original"; 

        //PRODUCTOS GOOGLE
		private static string nombreProductoCoin1 = "productoCoin6200";
		public static string productoCoin1Google = "coin_6200";

		private static string nombreProductoCoin2 = "productoCoin12500";
		public static string productoCoin2Google = "coin_12500";

		private static string nombreProductoCoin3 = "productoCoin32500";
		public static string productoCoin3Google = "coin_32500";

		private static string nombreProductoCoin4 = "productoCoin70000";
		public static string productoCoin4Google = "coin_70000";	
		
		private static string nombreProductoCoin5 = "productoCoin160000";
		public static string productoCoin5Google = "coin_160000";

		private static string nombreProductoCoin6 = "productoCoin500000";
		public static string productoCoin6Google = "coin_500000";

    void Start()
		{
			// If we haven't set up the Unity Purchasing reference
			if (m_StoreController == null)
			{
				// Begin to configure our connection to Purchasing
				InitializePurchasing();
			}
		}

		public void InitializePurchasing() 
		{
			// If we have already connected to Purchasing ...
			if (IsInitialized())
			{
				// ... we are done here.
				return;
			}

			// Create a builder, first passing in a suite of Unity provided stores.
			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			
			// Add a product to sell / restore by way of its identifier, associating the general identifier
			// with its store-specific identifiers.
			builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
			// Continue adding the non-consumable product.
			builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
			// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
			// if the Product ID was configured differently between Apple and Google stores. Also note that
			// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
			// must only be referenced here. 
			builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){{ kProductNameAppleSubscription, AppleAppStore.Name },{ kProductNameGooglePlaySubscription, GooglePlay.Name },});
			//My own google products --FRAN
			builder.AddProduct (nombreProductoCoin1, ProductType.Consumable, new IDs() { { productoCoin1Google, GooglePlay.Name }, });
			builder.AddProduct (nombreProductoCoin2, ProductType.Consumable, new IDs() { { productoCoin2Google, GooglePlay.Name }, });
			builder.AddProduct (nombreProductoCoin3, ProductType.Consumable, new IDs() { { productoCoin3Google, GooglePlay.Name }, });
			builder.AddProduct (nombreProductoCoin4, ProductType.Consumable, new IDs() { { productoCoin4Google, GooglePlay.Name }, });
			builder.AddProduct (nombreProductoCoin5, ProductType.Consumable, new IDs() { { productoCoin5Google, GooglePlay.Name }, });
			builder.AddProduct (nombreProductoCoin6, ProductType.Consumable, new IDs() { { productoCoin6Google, GooglePlay.Name }, });

			// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
			// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
			UnityPurchasing.Initialize(this, builder);
		}


		private bool IsInitialized()
		{
			// Only say we are initialized if both the Purchasing references are set.
			return m_StoreController != null && m_StoreExtensionProvider != null;
		}


		public void BuyConsumable()
		{
			// Buy the consumable product using its general identifier. Expect a response either 
			// through ProcessPurchase or OnPurchaseFailed asynchronously.
		BuyProductID(kProductIDConsumable);
		}
		
		public void BuyCoin6200(){

		BuyProductID (nombreProductoCoin1);
		}

		public void BuyCoin12500(){

			BuyProductID (nombreProductoCoin2);
		}
		
		public void BuyCoin32500(){

			BuyProductID (nombreProductoCoin3);
		}

		public void BuyCoin70000(){

			BuyProductID (nombreProductoCoin4);
		}

		public void BuyCoin160000(){

			BuyProductID (nombreProductoCoin5);
		}

		public void BuyCoin500000(){

			BuyProductID (nombreProductoCoin6);
		}


		public void BuyNonConsumable()
		{
			// Buy the non-consumable product using its general identifier. Expect a response either 
			// through ProcessPurchase or OnPurchaseFailed asynchronously.
			BuyProductID(kProductIDNonConsumable);
		}


		public void BuySubscription()
		{
			// Buy the subscription product using its the general identifier. Expect a response either 
			// through ProcessPurchase or OnPurchaseFailed asynchronously.
			// Notice how we use the general product identifier in spite of this ID being mapped to
			// custom store-specific identifiers above.
			BuyProductID(kProductIDSubscription);
		}


		void BuyProductID(string productId)
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing 
				// system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
					// asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation  
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
				// retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}


		// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
		// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
		public void RestorePurchases()
		{
			// If Purchasing has not yet been set up ...
			if (!IsInitialized())
			{
				// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
				Debug.Log("RestorePurchases FAIL. Not initialized.");
				return;
			}

			// If we are running on an Apple device ... 
			if (Application.platform == RuntimePlatform.IPhonePlayer || 
				Application.platform == RuntimePlatform.OSXPlayer)
			{
				// ... begin restoring purchases
				Debug.Log("RestorePurchases started ...");

				// Fetch the Apple store-specific subsystem.
				var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
				// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
				// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
				apple.RestoreTransactions((result) => {
					// The first phase of restoration. If no more responses are received on ProcessPurchase then 
					// no purchases are available to be restored.
					Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				});
			}
			// Otherwise ...
			else
			{
				// We are not running on an Apple device. No work is necessary to restore purchases.
				Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			}
		}


		//  
		// --- IStoreListener
		//

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			// Purchasing has succeeded initializing. Collect our Purchasing references.
			Debug.Log("OnInitialized: PASS");

			// Overall Purchasing system, configured with products for this application.
			m_StoreController = controller;
			// Store specific subsystem, for accessing device-specific store features.
			m_StoreExtensionProvider = extensions;
		}


		public void OnInitializeFailed(InitializationFailureReason error)
		{
			// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
			Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		}


		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
		{
			// A consumable product has been purchased by this user.
		if (String.Equals (args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal)) {
			Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
			//ScoreManager.score += 100;
		} else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin1, StringComparison.Ordinal)) {
			
			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 6200;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 6200 MONEDAS FUERON COMPRADAS");


		} else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin2, StringComparison.Ordinal)) {

			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 12500;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 12500 MONEDAS FUERON COMPRADAS");


		} else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin3, StringComparison.Ordinal)) {

			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 32500;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 32500 MONEDAS FUERON COMPRADAS");
		
		} else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin4, StringComparison.Ordinal)) {

			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 70000;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 70000 MONEDAS FUERON COMPRADAS");

		} else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin5, StringComparison.Ordinal)) {

			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 160000;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 160000 MONEDAS FUERON COMPRADAS");

		}else if (String.Equals (args.purchasedProduct.definition.id, nombreProductoCoin6, StringComparison.Ordinal)) {

			int monedas=PlayerPrefs.GetInt("PlayerMoney") + 500000;
			PlayerPrefs.SetInt ("PlayerMoney",monedas);
			GameObject.Find ("Dinero").GetComponent<Text> ().text = monedas.ToString(); 

			Debug.Log ("LAS 500000 MONEDAS FUERON COMPRADAS");

		}
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
			{
				Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
			}
			// Or ... a subscription product has been purchased by this user.
			else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
			{
				Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// TODO: The subscription item has been successfully purchased, grant this to the player.
			}
			// Or ... an unknown product has been purchased by this user. Fill in additional products here....
			else 
			{
				Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));}

			// Return a flag indicating whether this product has completely been received, or if the application needs 
			// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
			// saving purchased products to the cloud, and when that save is delayed. 
			return PurchaseProcessingResult.Complete;
		}


		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
			// this reason with the user to guide their troubleshooting actions.
			Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));}
	}
