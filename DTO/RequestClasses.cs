namespace DTO
{

    public abstract class RequestBase : DTOBase
    {
    }

    public enum RequestResult
    {
        /// <summary>成功</summary>
        Success = 0,

        /// <summary>失敗</summary>
        Failure = 10,

        /// <summary>金欠</summary>
        NotEnoughMoney = 20,

        /// <summary>食べ物が見つからない</summary>
        NotFoundFood = 30,
    }

    /// <summary>
    /// 食べ物リクエスト。
    /// あれも食べたい。
    /// これも食べたい。
    /// <results>
    ///   <result><see cref="RequestResult.NotEnoughMoney"/></result>
    ///   <result><see cref="RequestResult.NotFoundFood"/></result>
    /// </results>
    /// </summary>
    public class RequestFood : RequestBase
    {
        /// <summary>
        /// 食べ物名。
        /// 一品までだよ。
        /// </summary>
        public string FoodName { get; set; } = "";
    }
}
