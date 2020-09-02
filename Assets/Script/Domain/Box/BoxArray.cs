using System;
using ManeProject.Infrastructure.DB;
using UnityEngine;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// Box 配列要素
    /// </summary>
    public interface IBoxArray
    {
        /// <summary>
        /// Box の位置
        /// </summary>
        Vector3 BoxPosition { get; }

        /// <summary>
        /// Box のタイプ
        /// </summary>
        BoxType.IType BoxType { get; }

        /// <summary>
        /// 現在削除できる状況か
        /// </summary>
        bool IsDeletable { get; }

        /// <summary>
        /// 再生成された Box なのか
        /// </summary>
        bool IsRegenerated { get; }

        /// <summary>
        /// グループになるリストの番号
        /// </summary>
        int GroupListNum { get; }

        /// <summary>
        /// Box のゲームオブジェクト
        /// </summary>
        GameObject GameObj { get; }

        /// <summary>
        /// Box のネーム
        /// </summary>
        BoxName BoxName { get; }

        IBoxArray SetGameObj(GameObject gameObject);

        IBoxArray SetPosition(Vector3 position);

        IBoxArray UnSetGameObj();

        IBoxArray UnSetType();

        IBoxArray SetTypeWithRegenerate(BoxType.IType type);

        IBoxArray SetType(BoxType.IType type);

        IBoxArray SetGroupNum(int num);
    }

    /// <summary>
    /// Box 配列要素ファクトリー
    /// </summary>
    public static class BoxArrayFactory
    {
        /// <summary>
        /// Box 配列の要素を生成する
        /// </summary>
        /// <param name="boxPosition"></param>
        /// <param name="boxType"></param>
        /// <returns></returns>
        public static IBoxArray Create(
                Vector3 boxPosition,
                BoxType.IType boxType,
                int groupListNum,
                bool isDeleteable,
                bool isRegenerated,
                GameObject gameObject,
                BoxName boxName
            ) => new BoxArrayImpl(
                boxPosition,
                boxType,
                groupListNum,
                isDeleteable,
                isRegenerated,
                gameObject,
                boxName
            );

        private sealed class BoxArrayImpl : IBoxArray
        {
            /// <summary>
            /// Box の位置
            /// </summary>
            public Vector3 BoxPosition { get; }

            /// <summary>
            /// Box のタイプ
            /// </summary>
            public BoxType.IType BoxType { get; }

            /// <summary>
            /// 現在削除できる状態か
            /// </summary>
            public bool IsDeletable { get; }

            /// <summary>
            /// 現在削除できる状態か
            /// </summary>
            public bool IsRegenerated { get; }

            /// <summary>
            /// グループとなるリストの番号
            /// </summary>
            public int GroupListNum { get; }

            /// <summary>
            /// Box のネーム
            /// </summary>
            public BoxName BoxName { get; }

            /// <summary>
            /// Box のネーム
            /// </summary>
            public GameObject GameObj { get; }

            public IBoxArray SetGameObj(GameObject gameObject)
                => new BoxArrayImpl(
                    BoxPosition,
                    BoxType,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    gameObject,
                    BoxName
                    );

            public IBoxArray SetPosition(Vector3 position)
                => new BoxArrayImpl(
                    position,
                    BoxType,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    GameObj,
                    BoxName
                    );

            public IBoxArray UnSetGameObj()
                => new BoxArrayImpl(
                    BoxPosition,
                    BoxType,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    null,
                    BoxName
                    );

            public IBoxArray UnSetType()
                => new BoxArrayImpl(
                    BoxPosition,
                    null,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    GameObj,
                    BoxName
                    );

            public IBoxArray SetTypeWithRegenerate(BoxType.IType type)
                => new BoxArrayImpl(
                    BoxPosition,
                    type,
                    GroupListNum,
                    true,
                    true,
                    GameObj,
                    BoxName
                    );

            public IBoxArray SetType(BoxType.IType type)
                => new BoxArrayImpl(
                    BoxPosition,
                    type,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    GameObj,
                    BoxName
                    );

            public IBoxArray SetGroupNum(int num)
                => new BoxArrayImpl(
                    BoxPosition,
                    BoxType,
                    num,
                    IsDeletable,
                    IsRegenerated,
                    GameObj,
                    BoxName
                    );

            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="boxPosition"></param>
            /// <param name="boxType"></param>
            public BoxArrayImpl(
                    Vector3 boxPosition,
                    BoxType.IType boxType,
                    int groupListNum,
                    bool isDeletable,
                    bool isRegenerated,
                    GameObject gameObject,
                    BoxName boxName
                ) => (
                    BoxPosition,
                    BoxType,
                    GroupListNum,
                    IsDeletable,
                    IsRegenerated,
                    GameObj,
                    BoxName
                ) = (
                    boxPosition,
                    boxType,
                    groupListNum,
                    isDeletable,
                    isRegenerated,
                    gameObject,
                    boxName
                );
        }
    }
}