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

		private readonly string _systemMessage;

		private readonly string _apiKey;

		//会話履歴を保持するリスト
		//今回は送信の度にこのクラスをインスタンス化するので過去の会話は保持されない
		private readonly List<ChatGPTMessageModel> _messageList = new List<ChatGPTMessageModel>();

		public ChatGPTConnector(string apiKey, string systemMessage)
		{
			_apiKey = apiKey;
			_systemMessage = systemMessage;
			_messageList.Add(new ChatGPTMessageModel() { role = "system", content = _systemMessage });
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
		public string[] GetCommands()
		{
			string messageContent = GetMessage();
			string text = messageContent.Replace("\r", "").Replace("\n", "");
			string[] sepalater = { "[コマンド]", "[会話部分]" };
			string[] words = text.Split(sepalater, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 1) return null;
			string[] commands = words[0].Split(';');
			return commands;
		}

		public string GetConversation()
		{
			string messageContent = GetMessage();
			string text = messageContent.Replace("\r", "").Replace("\n", "");
			string[] sepalater = { "[コマンド]", "[会話部分]" };
			string[] words = text.Split(sepalater, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 1) return words[0];
			return words[1];
		}

	
	}
}
