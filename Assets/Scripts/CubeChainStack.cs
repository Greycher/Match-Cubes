using System.Collections.Generic;

namespace MatchCubes {

    public class CubeChainStack {

        public static CubeChainStack Instance {
            get {
                if (_instance == null) {
                    _instance = new CubeChainStack();
                }

                return _instance;
            }
        }

        private static CubeChainStack _instance;

        private Stack<CubeChain> _chainStack = new Stack<CubeChain>();

        public void Push(CubeChain chain) {
            chain.Reset();
            _chainStack.Push(chain);
        }

        public CubeChain PopOrCreate() {
            CubeChain chain;
            if (_chainStack.Count > 0) {
                chain = _chainStack.Pop();
            }
            else {
                chain = new CubeChain();
            }

            return chain;
        }
    }
}
