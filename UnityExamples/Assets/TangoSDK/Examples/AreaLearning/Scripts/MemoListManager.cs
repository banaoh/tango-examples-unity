using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Scene
{
	/// <summary>
	/// MemoList制御クラス
	/// </summary>
	public class MemoListManager : MonoBehaviour
	{

		public AreaLearningInGameController memoPrefabs;

		/// <summary>
		/// MemoList内コンテンツオブジェクト
		/// </summary>
		[SerializeField]
		private GameObject _memoListContent = null;

		/// <summary>
		/// _memo
		/// </summary>
		[SerializeField]
		private GameObject _memoNode = null;

		/// <summary>
		/// MemoNode内オブジェクト
		/// </summary>
		private Image _memoImage = null;
		private Text _memoName = null;

		/// <summary>
		/// Contents内オブジェクト
		/// </summary>
		private Text _memoDetail = null;

		/// <summary>
		/// アイテムマスタStruct
		/// </summary>
		private struct MemoListMaster
		{
			public int memoId;     // MemoId
			public string name;    // 名前
			public string detail;  // 詳細
			public Image img;
		}


		/// <summary>
		/// アイテムマスタリスト
		/// </summary>
		private List<MemoListMaster> _memoListMst = new List<MemoListMaster>();


		/// <summary>
		/// リストロードフラグ
		/// </summary>
		private bool _isLoad = false;

		/// <summary>
		/// デバッグデータロード処理
		/// TODO:後で削除
		/// </summary>
		private void DebugDataLoad()
		{
			// MemoMasterデータロード
			_memoListMst.Clear();
			for(int memoMstCnt = 0; memoMstCnt < memoPrefabs.m_markPrefabs.Length; memoMstCnt++)
			{
				MemoListMaster memoMstData = new MemoListMaster();
				memoMstData.memoId = memoMstCnt;
				memoMstData.name = memoPrefabs.m_markPrefabs[memoMstCnt].name;
				memoMstData.detail = memoPrefabs.m_markPrefabs[memoMstCnt].name;
				memoMstData.img = memoPrefabs.m_markPrefabs [memoMstCnt].GetComponent<Image> ();

				_memoListMst.Add(memoMstData);
			}
		}

		/// <summary>
		/// 初期化処理
		/// </summary>
		void Start()
		{
			

			// MemoNode内の各オブジェクトの取得
			_memoImage = _memoNode.transform.FindChild("MemoImg").GetComponent<Image>();
			_memoName = _memoNode.transform.FindChild("MemoName").GetComponent<Text>();

			// MemoList内の各オブジェクト取得
			_memoDetail = transform.FindChild("Contents/Detail").GetComponent<Text>();


			memoPrefabs = GameObject.Find ("UIController").GetComponent<AreaLearningInGameController>();


			// TODO:テストデータのロード
			DebugDataLoad();

			CreateMemoList();

		}

		/// <summary>
		/// アイテムリスト生成処理
		/// </summary>
		private void CreateMemoList()
		{
			bool isFirst = true;

			if (!_isLoad)
			{
				// ユーザが所持しているアイテムの種類の数だけノードを生成
				foreach (MemoListMaster memoData in _memoListMst)
				{
					if (memoData.memoId != null)
					{
						Debug.Log (memoData.img.mainTexture);
						_memoName.text = memoData.name;
						_memoImage.sprite = memoData.img.sprite;
						Debug.Log ("2");

						if (isFirst)
						{
							Debug.Log ("3");
							// 詳細部に１レコード目のデータの情報をセット
							_memoDetail.text = memoData.detail;
							memoPrefabs.SetCurrentMarkType (0);
							isFirst = false;
							Debug.Log ("4");
						}
						Debug.Log ("5");
						// 別クラスに定義している子オブジェクトをインスタンス化するための関数
						Button node = this.SetChild(_memoListContent,_memoNode).GetComponent<Button>();

						Debug.Log ("6");
						// 参照渡しだとAddListner時に値がうまくセットされないため値渡しに変換
						MemoListMaster data = memoData;

						// ノードクリック時に詳細が表示されるようにイベントを付与
						node.onClick.AddListener(() => MemoSetting(data));
						Debug.Log ("7");
					}
				}
				_isLoad = true;
			}
		}

		/// <summary>
		/// 詳細設定処理
		/// </summary>
		/// <param name="data"></param>
		/// <param name="cnt"></param>
		private void MemoSetting(MemoListMaster data)
		{
			_memoDetail.text = data.detail;
			memoPrefabs.SetCurrentMarkType (data.memoId);
		}

		/// <summary>
		/// 子オブジェクトセット処理
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public Transform SetChild(GameObject parent,GameObject child,string name = null)
		{
			// プレハブからインスタンスを生成
			GameObject obj = Instantiate(child);

			// 作成したオブジェクトを子として登録
			obj.transform.SetParent(parent.transform);

			obj.transform.localPosition = new Vector3(0f, 0f, 0f);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);

			// 作成したオブジェクトの名前にが(Clone)がつかないようにプレハブの名前を再付与
			obj.name = (name != null) ? name : child.name;

			return obj.transform;
		}
	}
}