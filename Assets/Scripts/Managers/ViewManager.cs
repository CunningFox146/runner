using Runner.UI;
using System.Collections;
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

            CurrentView = PushView(_startView);
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
            var viewStack = Inst._viewStack;

            var view = viewStack.Pop();
            view.Hide();
            CurrentView = viewStack.Peek();
            CurrentView.Show();
        }
    }
}