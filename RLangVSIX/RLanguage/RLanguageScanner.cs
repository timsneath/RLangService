using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RLanguagePackage
{
    internal class RLanguageScanner : IScanner
    {
        private IVsTextBuffer m_buffer;
        private int m_offset;
        string m_source;

        private enum ParseState
        {
            InText = 0,
            InSingleQuotes = 1,
            InDoubleQuotes = 2,
            InComment = 3
        }

        public RLanguageScanner(IVsTextBuffer buffer)
        {
            m_buffer = buffer;
        }

        private bool GetNextToken(int startIndex, TokenInfo tokenInfo, ref int state)
        {
            bool foundToken = false;
            int endIndex = -1;
            int index = startIndex;

            if (index < m_source.Length)
            {
                if (m_source[0] == '#')
                {
                    tokenInfo.StartIndex = startIndex;
                    tokenInfo.EndIndex = m_source.Length - 1;
                    tokenInfo.Color = TokenColor.Comment;
                    tokenInfo.Type = TokenType.LineComment;
                    foundToken = true;
                }
                else if (state == (int)ParseState.InSingleQuotes)
                {
                    // Find end quote. If found, set state to InText
                    // and return the quoted string as a single token.
                    // Otherwise, return the string to the end of the line
                    // and keep the same state.
                    endIndex = m_source.IndexOf('\'');
                    if (endIndex > -1)
                    {
                        tokenInfo.StartIndex = startIndex;
                        tokenInfo.EndIndex = endIndex;
                        tokenInfo.Color = TokenColor.String;
                        tokenInfo.Type = TokenType.String;
                        state = (int)ParseState.InText;
                    }
                    else
                    {
                        tokenInfo.StartIndex = startIndex;
                        tokenInfo.EndIndex = m_source.Length - 1;
                        tokenInfo.Color = TokenColor.String;
                        tokenInfo.Type = TokenType.String;
                        state = (int)ParseState.InSingleQuotes;
                    }
                    foundToken = true;
                }
                else if (state == (int)ParseState.InDoubleQuotes)
                {
                    // Find end quote. If found, set state to InText
                    // and return the quoted string as a single token.
                    // Otherwise, return the string to the end of the line
                    // and keep the same state.
                    endIndex = m_source.IndexOf('\"');
                    if (endIndex > -1)
                    {
                        tokenInfo.StartIndex = startIndex;
                        tokenInfo.EndIndex = endIndex;
                        tokenInfo.Color = TokenColor.String;
                        tokenInfo.Type = TokenType.String;
                        state = (int)ParseState.InText;
                    }
                    else
                    {
                        tokenInfo.StartIndex = startIndex;
                        tokenInfo.EndIndex = m_source.Length - 1;
                        tokenInfo.Color = TokenColor.String;
                        tokenInfo.Type = TokenType.String;
                        state = (int)ParseState.InDoubleQuotes;
                    }
                    foundToken = true;
                }
                else
                {
                    // Parse the token starting at index, returning the
                    // token's start and end index in tokenInfo, along with
                    // the token's type and color to use.
                    // If the token is a quoted string and the string continues
                    // on the next line, set state to InQuotes.
                    // If the token is a comment and the comment continues
                    // on the next line, set state to InComment.
                    tokenInfo.StartIndex = startIndex;
                    endIndex = m_source.IndexOfAny(new char[] { ' ', '\t', '(', ')', '='}, startIndex);
                    if (endIndex == -1)
                    {
                        foundToken = false;
                    }
                    else
                    {
                        var token = m_source.Substring(startIndex, endIndex - startIndex);
                        tokenInfo.EndIndex = endIndex;

                        if (IsRKeyword(token))
                        {
                            tokenInfo.Color = TokenColor.Keyword;
                            tokenInfo.Type = TokenType.Keyword;
                        }
                        else if (IsROperator(token))
                        {
                            tokenInfo.Color = TokenColor.Text;
                            tokenInfo.Type = TokenType.Operator;
                        }
                        else
                        {
                            tokenInfo.Color = TokenColor.Identifier;
                            tokenInfo.Type = TokenType.Identifier;
                        }
                        foundToken = true;
                    }
                }
            }
            return foundToken;
        }

        private bool IsROperator(string token)
        {
            return (token == "<-" || token == "=" || token == "+" || token == "-" || token == "*" || token == "/");
        }

        private bool IsRKeyword(string token)
        {
            return (token == "NULL" || token == "FALSE" || token == "TRUE");
        }

        bool IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            bool foundToken = false;

            if (tokenInfo != null)
            {
                foundToken = GetNextToken(m_offset, tokenInfo, ref state);

                if (foundToken)
                {
                    m_offset = tokenInfo.EndIndex + 1;
                }
            }
            return foundToken;
        }

        void IScanner.SetSource(string source, int offset)
        {
            m_offset = offset;
            m_source = source;
        }
    }
}
