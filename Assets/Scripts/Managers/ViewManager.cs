using Runner.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner.Managers
{
    public class ViewManager : Singleton<ViewManager>
    {
        [SerializeField] private View _startView;
        [SerializeField] private View[] _views;
        private Stack<View> _viewStack;

        public static View CurrentView { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _viewStack = new Stack<View>();
        }

        private void Start()
        {
            CurrentView = PushView(_startView);
        }

        void OnDestroy()
        {
            CurrentView = null;
        }

        public static View PushView<T>() where T : View
        {
            return PushView(Inst._views.Where(v => v is T).First());
        }

        public static View PushView(View view)
        {
            view.Show();
            CurrentView?.Hide();
            CurrentView = view;

            Inst._viewStack.Push(view);
            return view;
        }

        public static void PopView()
        {
            Stack<View> viewStack = Inst._viewStack;
            if (viewStack.Count == 0) return;

            View view = viewStack.Pop();
            view.Hide();
            if (viewStack.Count > 0)
            {
                CurrentView = viewStack.Peek();
                CurrentView.Show();
            }
        }
    }
}