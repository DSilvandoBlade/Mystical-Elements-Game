using UnityEngine;

namespace ElementTree
{
    public enum Element
    {
        DEFAULT = 0,
        PLASMA,
        FROST,
        CHARGE,
        FLORA
    }

    public enum Reactions
    {
        CANCEL = 0,
        EVAPORATE,
        GROWTH,
        ENERGIZE,
        CONDENSATE,
        BURST,
        STICK
    }

    [System.Serializable]
    public static class ElementClass
    {
        /// <summary>
        /// This is called to calculate how much percent of damage an enemy recieves depending on element.
        /// </summary>
        /// <param name="bodyElement"></param>
        /// <param name="recievingElement"></param>
        /// <returns></returns>
        public static float GetFetchResistance(Element bodyElement, Element recievingElement)
        {
            // 1 = 100% of taken damage
            // 0 = 0% of taken damage
            float resistance = 1f;
            Debug.Log(recievingElement + " --> " + bodyElement);

            switch (bodyElement)
            {
                case Element.DEFAULT:
                    resistance = 1f;
                    break;
                case Element.PLASMA:

                    switch (recievingElement)
                    {
                        case Element.PLASMA:
                            resistance = 0f;
                            break;
                        case Element.FLORA:
                            resistance = 0.5f;
                            break;
                        case Element.FROST:
                            resistance = 1.5f;
                            break;
                        default:
                            resistance = 1f;
                            break;
                    }

                    break;

                case Element.FROST:

                    switch (recievingElement)
                    {
                        case Element.FROST:
                            resistance = 0f;
                            break;
                        case Element.PLASMA:
                            resistance = 0.5f;
                            break;
                        case Element.CHARGE:
                            resistance = 1.5f;
                            break;
                        default:
                            resistance = 1f;
                            break;
                    }

                    break;

                case Element.CHARGE:

                    switch (recievingElement)
                    {
                        case Element.CHARGE:
                            resistance = 0f;
                            break;
                        case Element.FROST:
                            resistance = 0.5f;
                            break;
                        case Element.FLORA:
                            resistance = 1.5f;
                            break;
                        default:
                            resistance = 1f;
                            break;
                    }

                    break;

                case Element.FLORA:

                    switch (recievingElement)
                    {
                        case Element.FLORA:
                            resistance = 0f;
                            break;
                        case Element.CHARGE:
                            resistance = 0.5f;
                            break;
                        case Element.PLASMA:
                            resistance = 1.5f;
                            break;
                        default:
                            resistance = 1f;
                            break;
                    }

                    break;

                default:
                    break;
            }

            return resistance;
        }
    }
}