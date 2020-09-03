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

        /// <summary>
        /// ゲームオブジェクト設定
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        IBoxArray SetGameObj(GameObject gameObject);

        /// <summary>
        /// ポジション設定
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        IBoxArray SetPosition(Vector3 position);

        /// <summary>
        /// ゲームオブジェクト設定削除
        /// </summary>
        /// <returns></returns>
        IBoxArray UnSetGameObj();

        /// <summary>
        /// タイプ設定削除
        /// </summary>
        /// <returns></returns>
        IBoxArray UnSetType();

        /// <summary>
        /// タイプ設定、再生成されたボックスに設定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBoxArray SetTypeWithRegenerate(BoxType.IType type);

        /// <summary>
        /// タイプ設定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBoxArray SetType(BoxType.IType type);

        /// <summary>
        /// グループナンバー設定
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
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

            /// <summary>
            /// ゲームオブジェクト設定
            /// </summary>
            /// <param name="gameObject"></param>
            /// <returns></returns>
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

            /// <summary>
            /// ポジション設定
            /// </summary>
            /// <param name="position"></param>
            /// <returns></returns>
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

            /// <summary>
            /// ゲームオブジェクト設定削除
            /// </summary>
            /// <returns></returns>
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

            /// <summary>
            /// タイプ設定削除
            /// </summary>
            /// <returns></returns>
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

            /// <summary>
            /// タイプ設定、再生成されたボックスに設定
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
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

            /// <summary>
            /// タイプ設定
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
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

            /// <summary>
            /// グループナンバー設定
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
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