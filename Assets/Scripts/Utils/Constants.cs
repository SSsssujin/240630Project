namespace INeverFall
{
    public enum WeaponType
    {
        Unarmed,
        TwoHandSword,
        
    }
    
    // BossAnimation enum을 파라미터로 각 애니메이션의 이름을 가져올 수 있다. - Utils class 참고
    // int 값이 명시되어있는 항목들은 Animation 트랜지션 조건이니까 잘 관리해 줘야 된다.  
    public enum BossAnimation
    {
        None = -1,
        
        // Melee
        GroundAttack = 0,
        ArmSwingAttack = 1,
        
        // Long range
        Dash = 5,
        ThrowStone = 6,
        
        HitLeftShoulder,
        HitRightShoulder,
        
        Rise,
    }
}