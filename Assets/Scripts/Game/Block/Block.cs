using System;
using UnityEngine;

namespace BouncingBalls
{
    [RequireComponent(typeof(Draggable))]
    public class Block : MonoBehaviour, IInitializable
    {
        [SerializeField] private ObjectType _objectType;
        [SerializeField] private float _forceBounce = 1;
        [SerializeField] private int _crisstalChance = 1;

        private PlayField _playField;
        private ScreenPosition _screenPosition;

        private float _baseForceBounce;
        private int _baseCrisstalChance;
        private int _profitability;
        private int _multiplayProfitability;
        private int _profitabilityCristall = 1;

        private Collider _collider;
        private Cell _currentCell;

        public Cell CurrentCell => _currentCell;
        public PlayField PlayField => _playField;
        public ObjectType ObjectType => _objectType;

        public event Action<int> OnInitialize;
        public event Action<BounceScoreData> OnScoreEarned;
        public event Action<Block> Deleted;
        public event Action Bounced;
        public event Action OnPostInitialize;

        private void OnDisable()
        {
            ClicabilityOn();
        }

        private void Awake()
        {
            _baseForceBounce = _forceBounce;
            _baseCrisstalChance = _crisstalChance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            int maxChance = 100;

            if (collision.gameObject.TryGetComponent(out Ball ball))
            {
                if (!CurrentCell.IsStock)
                {
                    BounceScoreData scoreData = new
                    (
                        _multiplayProfitability * _profitability * ball.Profitability,
                        UnityEngine.Random.Range(0, maxChance) <= _crisstalChance ? _profitabilityCristall : 0,
                        collision.contacts[0].point
                    );

                    OnScoreEarned?.Invoke(scoreData);
                }

                Rigidbody rigidbodyBall = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 normalInSurface = -collision.contacts[0].normal;
                Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normalInSurface);
                rigidbodyBall.velocity = reflection.normalized * _forceBounce;

                Bounced?.Invoke();
            }
        }

        public void Initialize(int factor)
        {
            if (gameObject.TryGetComponent(out BoxCollider boxCollider))
                _collider = boxCollider;
            else
                _collider = GetComponent<MeshCollider>();

            _profitability = factor;
            _multiplayProfitability = 1;
            _forceBounce = _baseForceBounce;
            _crisstalChance = _baseCrisstalChance;

            OnInitialize?.Invoke(_profitability);
        }

        public void PostInitialize(PlayField playField)
        {
            _playField = playField;

            OnPostInitialize?.Invoke();
        }

        public void ChangeCell(Cell newCell)
        {
            if (_currentCell != null)
                _currentCell.ReleaseCell();

            _currentCell = newCell;

            if (_currentCell == null)
                return;

            _currentCell.TakeCell();
            ChangePosition(_currentCell.GetPointPosition());
        }

        public void DeleteBlock()
        {
            Deleted?.Invoke(this);
        }

        public void ClicabilityOn()
        {
            if (_collider != null)
                _collider.enabled = true;
        }

        public void ClicabilityOff()
        {
            if (_collider != null)
                _collider.enabled = false;
        }

        public void ChangeMultiplayProfitability(int multiplayProfitability)
        {
            _multiplayProfitability = multiplayProfitability;
        }

        public void ChangeForceBounce(int valueAdd)
        {
            _forceBounce += valueAdd;
        }

        public void ChangeCristallChance(int value)
        {
            _crisstalChance = value;

            if (_crisstalChance <= 0)
                _crisstalChance = _baseCrisstalChance;
        }

        private void ChangePosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        private void OnChangeScreenOrientation()
        {
            transform.position = new Vector3(_currentCell.transform.position.x, _currentCell.transform.position.y, transform.position.z);
        }
    }
}