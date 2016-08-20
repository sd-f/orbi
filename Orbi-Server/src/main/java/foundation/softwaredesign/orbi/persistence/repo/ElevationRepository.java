package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.ElevationEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.SingleResultType;

import java.math.BigDecimal;
import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public interface ElevationRepository extends EntityRepository<ElevationEntity, Long> {

    @Query(value = "" +
            "SELECT ST_Value(rast, 1, ST_SetSRID(ST_Point(?1,?1), 4236)) altitude\n" +
            "FROM elevation e\n" +
            "WHERE ST_Intersects(e.rast,ST_SetSRID(ST_Point(?1,?2), 4236))\n" +
            "  AND e.latitude = trunc(?1)\n" +
            "  AND e.longitude = trunc(?2);", isNative = true, singleResult = SingleResultType.OPTIONAL)
    Double getAltitude(Double latitude, Double longitude);
}
