using System;
using R3;

namespace Event
{
    public abstract class GameEventBase
    {
        public abstract IDisposable Subscribe(Action<Unit> onEvent);
        public abstract void ResetAll();
    }
    
    public class GameEvent<T> : GameEventBase
    {
        // イベント発行のためのSubject
        private Subject<Unit> _subject = new Subject<Unit>();

        // 複数のデータを保持するための変数
        private readonly T _initialValue;     // 初期値
        public T Value { get; private set; } // 現在の値

        // コンストラクタで初期値を設定
        public GameEvent(T initialValue)
        {
            this._initialValue = initialValue;
            Value = initialValue;
        }

        // イベントを発行
        public void Trigger(T data)
        {
            Value = data;
            _subject.OnNext(Unit.Default);
        }
    
        public void SetValue(T data)
        {
            Value = data;
        }
    
        public T GetValue()
        {
            return Value;
        }
    
        public T GetAndResetValue()
        {
            var v = Value;
            Reset();
            return v;
        }

        private void Reset()
        {
            Value = _initialValue;
        }

        // 購読機能
        public override IDisposable Subscribe(Action<Unit> onEvent)
        {
            return _subject.Subscribe(onEvent);
        }
    
        public override void ResetAll()
        {
            Reset();
            _subject = new Subject<Unit>();
        }
    }
}