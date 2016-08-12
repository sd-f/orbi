package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.virtual.Cube;
import foundation.softwaredesign.orbi.persistence.entity.CubeEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Modifying;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import java.math.BigDecimal;
import java.math.BigInteger;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = CubeEntity.class)
@MappingConfig(CubeMappper.class)
public interface CubeRepository extends EntityRepository<Cube, BigInteger> {

    @Modifying
    @Query("delete from CubeEntity")
    void deleteAll();

    @Query(" select e" +
            "  from CubeEntity e" +
            " where ( e.longitude between (?2 - 0.001) and (?2 + 0.001))" +
            "   and ( e.latitude between (?1 - 0.001) and (?1 + 0.001))")
    List<Cube> findCubesAround(BigDecimal latitude, BigDecimal longitude);
}
