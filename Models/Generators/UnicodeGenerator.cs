using System;

namespace EndApi.Models.Generators
{
    public class UnicodeGenerator
    {
        public static string GetUnicode(){
            string code = string.Empty;
            for(var i=0; i <3; i++){
                code += $"-{Guid.NewGuid().ToString().Split("-")[1]}";
            }
            return code.Substring(1).ToUpper();
        }
    }
}