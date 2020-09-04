using System;
using UnityEngine;

namespace ManeProject.Common
{
    public static class Common
    {
        public const int DEFAULT_MAX_COLUMN = 8;

        public const int DEFAULT_MAX_ROW = 8;

        public const int REGENERATED_Y = 10;

        public const int COLOR_COUNT = 3;

        public const int GAME_END_SCORE = 1000000;

        public const int BOX_SCORE = 10000;
    }

    public static class SceneCommon
    {
        public enum SceneInfo
        {
            StartScene,
            LoadingScene,
            GameMain,
            ResultScene,
        }
    }
}