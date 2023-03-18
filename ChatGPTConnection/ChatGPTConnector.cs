using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatGPTConnection
{
	public class ChatGPTConnector
	{

		private readonly string systemMessage = 
			"以下の条件に従って、チャットボットとしてロールプレイをします。\r\n\r\n" +
			"あなたは通常の会話に加え、オブジェクトの操作を行うことができます。\r\n" +
			"オブジェクトの操作を行う場合には、コマンドを出力してください。\r\n" +
			"操作がない場合にはコマンドは空白としてください。\r\n\r\n" +
			"利用可能なコマンドは以下の通りです。\r\n\r\n- Box location sizeX sizeY sizeZ\r\n" +
			"ボックスを作成します。\r\n" +
			"locationはボックスを生成する位置で3次元の座標です。\r\n" +
			"sizeX, sizeY, sizeZはそれぞれ数値です。\r\n\r\n" +
			"-Sphere location radius\r\n" +
			"球を作成します。\r\n" +
			"locationは球を生成する位置で3次元の座標です。\r\n" +
			"radiusは球の半径で数値です。\r\n\r\n" +
			"-SetName name\r\n" +
			"名前を設定します。\r\n" +
			"nameは設定するオブジェクトの名前で文字列です。\r\n\r\n" +
			"-Polyline positionArray Enter\r\n" +
			"ポリラインを作成します。\r\n" +
			"positionArrayは線を結ぶ点の座標の配列です。\r\n" +
			"最後にEnterを必ず入れます。\r\n\r\n" +
			"-SelectObject name\r\n" +
			"オブジェクトを選択します。\r\n" +
			"nameは選択するオブジェクトの名前で文字列です。\r\n\r\n" +
			"-Move base vector\r\n" +
			"オブジェクトを移動します。\r\n" +
			"baseは基点となる3次元の座標です。\r\n" +
			"vectorは移動する方向です。\r\n\r\n" +
			"-Scale Enter scale\r\n" +
			"オブジェクトを拡大・縮小します。\r\n" +
			"scaleは倍率です。\r\n\r\n" +
			"-Delete\r\n" +
			"オブジェクトを削除します。\r\n\r\n" +
			"利用可能なコマンドは以上です。以上のコマンド以外(forループなど)は絶対に使用しないでください。\r\n\r\n今後の会話では、あなたは常にコマンドを[コマンド]以下に記載し、会話内容については[会話部分]に記載して語尾に「にゃ！」をつけて返答してください。以下は「0,1,3に10 20 20のボックスを生成する」という指示に対する回答例です。\r\n\r\n[コマンド]\r\nBox 0,1,3 10 20 20\r\n\r\n[会話部分]\r\n10 20 20のボックスを0,1,3に生成したにゃ！\r\n\r\nまた、複数のコマンドの場合は必ずセミコロン「；」で区切ってください。以下が「\"test_01\"を0,0,0を基点として10,10,10移動させる」という指示に対する回答例です。\r\n\r\n[コマンド]\r\nSelectObject test_01;\r\nMove 0,0,0 10,10,10;\r\n\r\n[会話部分]\r\ntest_01を0,0,0を基点に10,10,10移動させたにゃ！\r\n\r\n以下は「0,0,0から10,10,10の範囲内の位置に1 1 1から3 3 3の範囲内のサイズのボックスを3つ生成する」という指示対する回答例です。\r\n\r\n[コマンド]\r\nBox 2,3,4 1.2 2.4 1.7;\r\nBox 1,6,8 2.2 2.8 1.2;\r\nBox 7,4,3 2.6 1.1 1.8;\r\n\r\n[会話部分]\r\n0,0,0から10,10,10の範囲内の位置に1 1 1から3 3 3の範囲内のサイズのボックスを3つ生成したにゃ！\r\n\r\n以下は「これをもう少し大きくする」という指示に対する回答例です。\r\n\r\n[コマンド]\r\nScale Enter 1.2\r\n\r\n[会話部分]\r\nもう少し大きくしたにゃ！";

		private readonly string _apiKey;

		//会話履歴を保持するリスト
		private readonly List<ChatGPTMessageModel> _messageList = new List<ChatGPTMessageModel>();

		public ChatGPTConnector(string apiKey)
		{
			_apiKey = apiKey;
			_messageList.Add(new ChatGPTMessageModel() { role = "system", content = systemMessage });
		}

		public async Task<ChatGPTResponseModel> RequestAsync(string userMessage)
		{
			//文章生成AIのAPIのエンドポイントを設定
			var apiUrl = "https://api.openai.com/v1/chat/completions";
			_messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

			//OpenAIのAPIリクエストに必要なヘッダー情報を設定
			var headers = new Dictionary<string, string>
			{
				{"Authorization", "Bearer " + _apiKey},
				{"X-Slack-No-Retry", "1"}
			};

			//文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
			var options = new ChatGPTCompletionRequestModel()
			{
				model = "gpt-3.5-turbo",
				messages = _messageList
			};

			var jsonOptions = JsonConvert.SerializeObject(options);

			var httpClient = new HttpClient();

			foreach (var header in headers)
			{
				httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
			}

			var responseString = await httpClient.PostAsync(apiUrl, new StringContent(jsonOptions, Encoding.UTF8, "application/json"));

			if (!responseString.IsSuccessStatusCode)
			{
				return new ChatGPTResponseModel() {isSuccess = false };	
			}
			else
			{
				var content = await responseString.Content.ReadAsStringAsync();
				var responseObject = JsonConvert.DeserializeObject<ChatGPTResponseModel>(content);
				responseObject.isSuccess = true;
				return responseObject;
			}
		}



	}


	public class ChatGPTMessageModel
	{
		public string role;
		public string content;
	}

	//ChatGPT APIにRequestを送るためのJSON用クラス
	public class ChatGPTCompletionRequestModel
	{
		public string model;
		public List<ChatGPTMessageModel> messages;
	}

	//ChatGPT APIからのResponseを受け取るためのクラス
	public class ChatGPTResponseModel
	{
		public bool isSuccess;
		public string id;
		public string @object;
		public int created;
		public Choice[] choices;
		public Usage usage;

		public class Choice
		{
			public int index;
			public ChatGPTMessageModel message;
			public string finish_reason;
		}

		public class Usage
		{
			public int prompt_tokens;
			public int completion_tokens;
			public int total_tokens;
		}

		public string GetMessage()
		{
			return this.choices[0].message.content;
		}

		//ChatGPTから受け取ったメッセージをパースする
		//TODO: もう少しうまくつくりたい
		public RhinoCommand GetCommands()
		{
			string messageContent = GetMessage();
			string text = messageContent.Replace("\r", "").Replace("\n", "");
			string[] sepalater = { "[コマンド]", "[会話部分]" };
			string[] words = text.Split(sepalater, StringSplitOptions.RemoveEmptyEntries);
			string[] commands = words[0].Split(';');
			return new RhinoCommand(commands);
		}

		public string GetConversation()
		{
			string messageContent = GetMessage();
			string text = messageContent.Replace("\r", "").Replace("\n", "");
			string[] sepalater = { "[コマンド]", "[会話部分]" };
			string[] words = text.Split(sepalater, StringSplitOptions.RemoveEmptyEntries);
			return words[1];
		}

	
	}
}
